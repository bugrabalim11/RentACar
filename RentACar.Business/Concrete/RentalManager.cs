using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Business.Helpers;
using RentACar.Core.Aspects.Autofac.Transaction;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Mailing;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarDtos;
using RentACar.Dtos.CustomerDtos;
using RentACar.Dtos.PaymentDtos;
using RentACar.Dtos.RentalDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class RentalManager : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;              
        private readonly IPosService _posService;           
        private readonly IPaymentService _paymentService;
        private readonly IMailService _mailService;
        private readonly ICarService _carService;
        private readonly ICarStatusService _carStatusService;
        private readonly ICustomerService _customerService;
        private readonly RentalBusinessRules _rules;

        public RentalManager(IRentalRepository rentalRepository, IMapper mapper, IPosService posService, IPaymentService paymentService, IMailService mailService, ICarService carService, ICarStatusService carStatusService, ICustomerService customerService, RentalBusinessRules rules)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _posService = posService;
            _paymentService = paymentService;
            _mailService = mailService;
            _carService = carService;
            _carStatusService = carStatusService;
            _customerService = customerService;
            _rules = rules; 
        }

        [TransactionScopeAspect]
        public async Task<IDataResult<int>> AddAsync(RentalCreateDto rentalAddDto, int userId)
        {
            // ===================================================================================
            // BÖLÜM 1: ÖN HAZIRLIK VE GÜVENLİK (Preparation)
            // ===================================================================================

            // 1. Tarih Formatlama (UTC Koruması)
            rentalAddDto.RentDate = DateTime.SpecifyKind(rentalAddDto.RentDate, DateTimeKind.Utc);
            if (rentalAddDto.ReturnDate.HasValue)
            {
                rentalAddDto.ReturnDate = DateTime.SpecifyKind(rentalAddDto.ReturnDate.Value, DateTimeKind.Utc);
            }

            // 2. Kimlik Tespiti (Vatandaşı Müşteriye Çevirme)
            var customerResult = await _customerService.GetMyCustomerProfileAsync(userId);
            if (!customerResult.Success)
            {
                throw new BusinessException("Kiralama yapabilmek için lütfen ilk önce müşteri profilinizi oluşturun!");
            }

            // ===================================================================================
            // BÖLÜM 2: İŞ KURALLARI DUVARI (Validation & Business Rules)
            // ===================================================================================

            // Kurallardan biri bile patlarsa kod aşağıya inmez, işlem anında kesilir.
            IResult? result = BusinessRules.Run(
            _rules.CheckIfRentDateBeforeToday(rentalAddDto.RentDate),
            await _carService.CheckIfCarExistsAsync(rentalAddDto.CarId),
            await _carStatusService.CheckIfCarIsInMaintenanceAsync(rentalAddDto.CarId, rentalAddDto.RentDate, rentalAddDto.ReturnDate),
            await _rules.CheckIfCustomerDrivingExperienceIsSufficient(rentalAddDto.CarId, customerResult.Data.Id),
            await _rules.CheckIfCustomerFindexScoreIsSufficient(rentalAddDto.CarId, customerResult.Data.Id),
            await _rules.CheckIfCarAvailable(rentalAddDto.CarId, rentalAddDto.RentDate, rentalAddDto.ReturnDate)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // ===================================================================================
            // BÖLÜM 3: FİNANS VE HARİCİ SİSTEMLER (Finance & POS)
            // ===================================================================================

            // 1. Veri Hazırlığı ve Zımbalama
            var rental = _mapper.Map<Rental>(rentalAddDto);
            rental.CustomerId = customerResult.Data.Id;

            // 2. Tutar Hesaplama
            var car = await _carService.GetByIdAsync(rental.CarId);
            decimal totalAmount = CalculateTotalAmount(rental.RentDate, rental.ReturnDate, car.Data.DailyPrice);
            rental.TotalAmount = totalAmount;

            // 3. POS Cihazı ile Tahsilat
            // DİKKAT: Veritabanına dokunmadan ÖNCE parayı çekiyoruz. 
            // Kartta para yoksa sistem burada hata fırlatır ve DB yorulmaz.
            var posResult = await _posService.PayAsync(rentalAddDto.CreditCardInformation, totalAmount);
            if (!posResult.Success)
            {
                throw new BusinessException(posResult.Message ?? "Ödeme sırasında bir hata oluştu, lütfen tekrar deneyin!");
            }

            // ===================================================================================
            // BÖLÜM 4: VERİTABANI İŞLEMLERİ (Database Writes - Transaction Koruması Altında)
            // ===================================================================================

            // Eğer buralarda SQL çökerse, tepedeki [TransactionScopeAspect] zamanı geri sarar!

            // 1. Araç Kiralama Fişini Kes (Rental)
            await _rentalRepository.AddAsync(rental);

            // 2. Kasa Defterine Dekont Yaz (Payment)
            var paymentDto = new PaymentCreateDto
            {
                RentalId = rental.Id, // Bir üst satırda AddAsync çalıştığı için bu Id artık 0 değil!
                Amount = totalAmount,
                TransactionId = Guid.NewGuid().ToString(), // Şimdilik uydurma banka işlem numarası
                IsSuccessful = true
            };
            await _paymentService.AddAsync(paymentDto);

            // ===================================================================================
            // BÖLÜM 5: BÖLÜM 5: E-POSTA BİLDİRİMİ (İzole Odacık - Transaction'ı Patlatmaması İçin)
            // ===================================================================================

            // Sadece KUTULARI fırlat, asistan kendi içinde zımbalasın!
            await SendRentalSuccessEmailAsync(
                customerResult.Data,
                car.Data,
                totalAmount,
                rental.RentDate,
                rental.ReturnDate
                );

            // Postman/UI otomasyonları için üretilen yeni ID'yi teslim et.
            return new SuccessDataResult<int>(rental.Id, "Araç kiralama başarıyla oluşturuldu.");
        }

        public async Task<IDataResult<List<RentalResultDto>>> GetAllByUserIdAsync(int userId)
        {
            var rentals = await _rentalRepository.GetRentalsByUserIdAsync(userId);
            if (rentals == null || !rentals.Any())
            {
                throw new BusinessException("Kullanıcıya ait kiralama işlemleri bulunamadı.");
            }

            var sortedRentals = rentals.OrderByDescending(x => x.CreatedDate).ToList();
            var mappedRentals = _mapper.Map<List<RentalResultDto>>(sortedRentals);
            return new SuccessDataResult<List<RentalResultDto>>(mappedRentals, "Kullanıcıya ait kiralama işlemleri başarıyla listelendi.");
        }

        public async Task<IDataResult<RentalDetailDto>> GetMyRentalByIdAsync(int rentalId, int userId)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(rentalId);
            if (rental == null)
            {
                throw new BusinessException("Aradağınız kiralama bulunamadı!");
            }

            if (rental.Customer.UserId != userId)
            {
                throw new BusinessException("Güvenlik İhlali: Bu kiralama kaydını (faturayı) görüntüleme yetkiniz yok!");
            }

            var mappedRental = _mapper.Map<RentalDetailDto>(rental);
            return new SuccessDataResult<RentalDetailDto>(mappedRental, "Kiralama detaylarınız başarıyla getirildi.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> UpdateMyRentalAsync(int userId, int rentalId, RentalUpdateReturnDateDto rentalUpdateReturnDateDto)
        {
            rentalUpdateReturnDateDto.ReturnDate = DateTime.SpecifyKind(rentalUpdateReturnDateDto.ReturnDate, DateTimeKind.Utc);

            var existingRental = await _rentalRepository.GetRentalWithDetailsByIdAsync(rentalId);
            if (existingRental == null) return new ErrorResult("Kiralama bulunamadı!");

            if (existingRental.Customer.UserId != userId)
            {
                throw new BusinessException("Bu kiralamayı güncellemeye yetkiniz yok!");
            }

            IResult? result = BusinessRules.Run
            (
                _rules.CheckIfRentalIsAlreadyCompleted(existingRental.ReturnDate),
                await _rules.CheckIfCarAvailableForUpdate(rentalId, existingRental.CarId, existingRental.RentDate, rentalUpdateReturnDateDto.ReturnDate)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var newTotalAmount = CalculateTotalAmount(existingRental.RentDate, rentalUpdateReturnDateDto.ReturnDate, existingRental.Car.DailyPrice);
            var difference = newTotalAmount - existingRental.TotalAmount;
            // SENİOR NOTU: Müşteri aracı erken teslim ettiğinde (difference < 0) para iadesi YAPILMAMAKTADIR.
            // Çünkü aracın o tarihler arası başka müşteriye kiralanma fırsatı (Fırsat Maliyeti) baltalanmıştır.
            // İş kuralı gereği bu durum bilerek göz ardı edilmiştir.
            // SENİOR NOTU: Müşteri aracı erken teslim ettiğinde para iadesi YAPILMAMAKTADIR.
            // Bu yüzden SADECE fiyat farkı pozitifse (süre uzamışsa) işlem yapıyoruz.
            if (difference > 0)
            {
                // 1. POS'tan çekim yap...
                var posResult = await _posService.PayAsync(rentalUpdateReturnDateDto.CreditCardInformation, difference);
                if (!posResult.Success)
                {
                    throw new BusinessException(posResult.Message ?? "Ödeme sırasında bir hata oluştu, lütfen tekrar deneyin!");
                }

                // 2. Ödeme başarılıysa PaymentDto (Fiş) oluştur ve kaydet...
                // Yeni farklı bir kes
                var paymentDto = new PaymentCreateDto
                {
                    RentalId = existingRental.Id,
                    Amount = difference,
                    TransactionId = Guid.NewGuid().ToString(),
                    IsSuccessful = true
                };
                await _paymentService.AddAsync(paymentDto);

                // 3. BOMBA DOKUNUŞ BURADA: Madem sadece fiyat artınca güncelleyeceğiz,
                // Rental'ın TotalAmount atamasını sadece bu bloğun içine alıyoruz!
                existingRental.TotalAmount = newTotalAmount;
            }

            // Elimizdeki çamurlu kasaları (Entity), asistanın istediği şık vitrin kutularına (DTO) çeviriyoruz!
            var carDto = _mapper.Map<CarDetailDto>(existingRental.Car);
            var customerDto = _mapper.Map<CustomerDetailDto>(existingRental.Customer);

            await SendRentalUpdateEmailAsync(
            customerDto,
            carDto,
            existingRental.TotalAmount,
            difference > 0 ? difference : 0, // Eğer difference 0'dan büyükse difference'ın kendisini kullan; DEĞİLSE 0 kullan.
            existingRental.RentDate,
            rentalUpdateReturnDateDto.ReturnDate
            );

            // Fark 0 veya negatifse (difference <= 0), TotalAmount'a HİÇ DOKUNMUYORUZ.
            // O zaten veritabanından 1000 TL olarak geldi, öyle kalacak. Paraya çöktük :)

            // Aşağıda sadece tarihi güncelliyor ve SQL'e gönderiyoruz:
            existingRental.ReturnDate = rentalUpdateReturnDateDto.ReturnDate;
            await _rentalRepository.UpdateAsync(existingRental);
            return new SuccessResult("Araç teslim tarihiniz başarıyla güncellendi.");
        }

        private decimal CalculateTotalAmount(DateTime rentDate, DateTime? returnDate, decimal dailyPrice)
        {
            decimal totalDays = 1;
            if (returnDate.HasValue)
            {
                var timeSpan = returnDate.Value - rentDate;

                // SENİOR NOTU: Neden (decimal) ile Casting (Bilinçli Tip Dönüşümü) yaptık?
                // Math.Ceiling fonksiyonu bize 'double' döner (Bilim İnsanı mantığı: virgüllü ve esnek).
                // Ancak para işlemleri (dailyPrice) sıfır hata payı isteyen 'decimal' (Muhasebeci mantığı) tipindedir.
                // C# derleyicisi bu iki farklı dünyanın çarpılmasına güvenlik gereği izin vermez.
                // Bu yüzden (decimal) yazarak "Bilim insanının yuvarladığı sayıyı, muhasebecinin diline çevir" dedik.
                totalDays = (decimal)Math.Ceiling(timeSpan.TotalDays);

                if (totalDays == 0 || totalDays < 0) { totalDays = 1; }
            }
            var totalAmount = totalDays * dailyPrice;
            return totalAmount;
        }

        // Veznedarın fatura ve kargo işlerini halleden özel asistanı
        private async Task SendRentalSuccessEmailAsync(CustomerDetailDto customer, CarDetailDto car, decimal totalAmount, DateTime rentDate, DateTime? returnDate)
        {
            try
            {
                // 1. Matbaacıdan hazır kargo kutusunu (Zarfı) al
                var mailRequest = MailTemplateHelper.CreateRentalSuccessMail(
                    $"{customer.FirstName} {customer.LastName}",
                    customer.Email,
                    $"{car.BrandName} {car.ModelName}",
                    totalAmount,
                    rentDate,
                    returnDate);

                // 2. Postacıya teslim et ve yola çıkar
                await _mailService.SendEmailAsync(mailRequest);
            }
            catch (Exception)
            {
                // Mail atılamasa bile kiralama işlemi iptal OLMASIN! 
                // Hata yutulur (İstersen buraya ileride NLog ile loglama yapabiliriz).
            }
        }

        private async Task SendRentalUpdateEmailAsync(CustomerDetailDto customer, CarDetailDto car, decimal totalAmount, decimal differenceAmount, DateTime rentDate, DateTime? returnDate)
        {
            try
            {
                // 1. Matbaacıdan hazır kargo kutusunu (Zarfı) al
                var mailRequest = MailTemplateHelper.CreateRentalUpdateMail(
                    $"{customer.FirstName} {customer.LastName}",
                    customer.Email,
                    $"{car.BrandName} {car.ModelName}",
                    totalAmount,
                    differenceAmount,
                    rentDate,
                    returnDate);

                await _mailService.SendEmailAsync(mailRequest);
            }
            catch (Exception)
            {

            }
        }
    }
}