using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Helpers;
using RentACar.Business.Rules;
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
    public class AdminRentalManager : IAdminRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        private readonly IAdminCustomerService _adminCustomerService;
        private readonly ICarService _carService;
        private readonly ICarStatusService _carStatusService;
        private readonly IPosService _posService;
        private readonly IPaymentService _paymentService;
        private readonly IMailService _mailService;
        private readonly RentalBusinessRules _rentalBusinessRules;

        public AdminRentalManager(IRentalRepository rentalRepository, IMapper mapper, IAdminCustomerService adminCustomerService, ICarService carService, ICarStatusService carStatusService, IPosService posService, IPaymentService paymentService, IMailService mailService, RentalBusinessRules rentalBusinessRules)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _adminCustomerService = adminCustomerService;
            _carService = carService;
            _carStatusService = carStatusService;
            _posService = posService;
            _paymentService = paymentService;
            _mailService = mailService;
            _rentalBusinessRules = rentalBusinessRules;
        }

        [TransactionScopeAspect]
        public async Task<IDataResult<int>> AddByAdminAsync(RentalCreateByAdminDto rentalAddByAdminDto)
        {
            // ===================================================================================
            // BÖLÜM 1: ÖN HAZIRLIK VE GÜVENLİK (Preparation)
            // ===================================================================================

            // Tarih Formatlama (UTC Koruması)  
            rentalAddByAdminDto.RentDate = DateTime.SpecifyKind(rentalAddByAdminDto.RentDate, DateTimeKind.Utc);
            if (rentalAddByAdminDto.ReturnDate.HasValue)
            {
                rentalAddByAdminDto.ReturnDate = DateTime.SpecifyKind(rentalAddByAdminDto.ReturnDate.Value, DateTimeKind.Utc);
            }

            // ===================================================================================
            // BÖLÜM 2: İŞ KURALLARI DUVARI (Validation & Business Rules)
            // ===================================================================================

            // Kurallardan biri bile patlarsa kod aşağıya inmez, işlem anında kesilir.
            IResult? result = BusinessRules.Run(
            _rentalBusinessRules.CheckIfRentDateBeforeToday(rentalAddByAdminDto.RentDate),
            await _rentalBusinessRules.CheckIfCustomerExistsAsync(rentalAddByAdminDto.CustomerId),
            await _rentalBusinessRules.CheckIfCarExistsAsync(rentalAddByAdminDto.CarId),
            await _rentalBusinessRules.CheckIfCarIsInMaintenanceAsync(rentalAddByAdminDto.CarId, rentalAddByAdminDto.RentDate, rentalAddByAdminDto.ReturnDate),
            await _rentalBusinessRules.CheckIfCustomerDrivingExperienceIsSufficient(rentalAddByAdminDto.CarId, rentalAddByAdminDto.CustomerId),
            await _rentalBusinessRules.CheckIfCustomerFindexScoreIsSufficient(rentalAddByAdminDto.CarId, rentalAddByAdminDto.CustomerId),
            await _rentalBusinessRules.CheckIfCarAvailable(rentalAddByAdminDto.CarId, rentalAddByAdminDto.RentDate, rentalAddByAdminDto.ReturnDate)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // ===================================================================================
            // BÖLÜM 3: FİNANS VE HARİCİ SİSTEMLER (Finance & POS)
            // ===================================================================================

            // 1. Veri Hazırlığı 
            var rental = _mapper.Map<Rental>(rentalAddByAdminDto);

            // 2. Tutar Hesaplama
            var car = await _carService.GetByIdAsync(rental.CarId);
            decimal totalAmount = CalculateTotalAmount(rental.RentDate, rental.ReturnDate, car.Data.DailyPrice);
            rental.TotalAmount = totalAmount;

            // 3. POS Cihazı ile Tahsilat
            // DİKKAT: Veritabanına dokunmadan ÖNCE parayı çekiyoruz. 
            // Kartta para yoksa sistem burada hata fırlatır ve DB yorulmaz.
            var posResult = await _posService.PayAsync(rentalAddByAdminDto.CreditCardInformation, totalAmount);
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
                RentalId = rental.Id,
                Amount = totalAmount,
                TransactionId = Guid.NewGuid().ToString(),
                IsSuccessful = true
            };
            await _paymentService.AddAsync(paymentDto);

            // ===================================================================================
            // BÖLÜM 5: ÇIKIŞ (Return)
            // ===================================================================================

            // İş kurallarında kullanıcı var mı dite kontrol etmiştik bir daha etmemize gerek yok.
            // E-posta ve isim soyisim bilgileri için bu veriyi çektik
            var customerResult = await _adminCustomerService.GetByIdAsync(rentalAddByAdminDto.CustomerId);

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

        [TransactionScopeAspect]
        public async Task<IResult> DeleteAsync(int id)
        {
            var existingRental = await _rentalRepository.GetAsync(x => x.Id == id);
            if (existingRental == null)
            {
                throw new BusinessException("Silinecek araç kiralama bulunamadı.");
            }

            await _rentalRepository.DeleteAsync(existingRental);
            return new SuccessResult("Araç kiralama başarıyla silindi.");
        }

        public async Task<IDataResult<List<RentalResultDto>>> GetAllAsync()
        {
            var rentals = await _rentalRepository.GetRentalsWithDetailsAsync();
            var sortedORentals = rentals.OrderByDescending(x => x.CreatedDate).ToList();
            var rentalsListDtos = _mapper.Map<List<RentalResultDto>>(sortedORentals);
            return new SuccessDataResult<List<RentalResultDto>>(rentalsListDtos, "Kiralama işlemleri başarıyla listelendi.");
        }

        public async Task<IDataResult<RentalDetailDto>> GetByIdAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(id);
            if (rental == null)
            {
                throw new BusinessException("Aranan araç kiralama bulunamadı.");
            }

            var rentalDetailDto = _mapper.Map<RentalDetailDto>(rental);
            return new SuccessDataResult<RentalDetailDto>(rentalDetailDto, "Araç kiralama detayı getirildi.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> UpdateByAdminAsync(RentalUpdateByAdminDto rentalUpdateDto)
        {
            // AsNoTracking kullandığımız için GetRentalWithDetailsByIdAsync metodu yerine GetAsync metodunu kullanıyoruz.
            // Çünkü c# asnotracing ile takibi bırakıyor ve update işlemi için entity framework'ün takibine ihtiyaç duyuyoruz.
            var existingRental = await _rentalRepository.GetAsync(x => x.Id == rentalUpdateDto.Id);
            if (existingRental == null)
            {
                throw new BusinessException("Güncellenecek araç kiralama bulunamadı.");
            }

            rentalUpdateDto.RentDate = rentalUpdateDto.RentDate.ToUniversalTime();
            if (rentalUpdateDto.ReturnDate.HasValue)
            {
                rentalUpdateDto.ReturnDate = rentalUpdateDto.ReturnDate.Value.ToUniversalTime();
            }

            IResult? result = BusinessRules.Run(
            _rentalBusinessRules.CheckIfRentalIsAlreadyCompleted(existingRental.ReturnDate),
            _rentalBusinessRules.CheckIfRentDateBeforeToday(rentalUpdateDto.RentDate),
            await _rentalBusinessRules.CheckIfCarExistsAsync(rentalUpdateDto.CarId),
            await _rentalBusinessRules.CheckIfCarIsInMaintenanceAsync(rentalUpdateDto.CarId, rentalUpdateDto.RentDate, rentalUpdateDto.ReturnDate),
            await _rentalBusinessRules.CheckIfCustomerDrivingExperienceIsSufficient(rentalUpdateDto.CarId, existingRental.CustomerId),
            await _rentalBusinessRules.CheckIfCustomerFindexScoreIsSufficient(rentalUpdateDto.CarId, existingRental.CustomerId),
            await _rentalBusinessRules.CheckIfCarAvailableForUpdate(rentalUpdateDto.Id, rentalUpdateDto.CarId, rentalUpdateDto.RentDate, rentalUpdateDto.ReturnDate)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            _mapper.Map(rentalUpdateDto, existingRental);

            var carResult = await _carService.GetByIdAsync(rentalUpdateDto.CarId);
            var newTotalAmount = CalculateTotalAmount(rentalUpdateDto.RentDate, rentalUpdateDto.ReturnDate, carResult.Data.DailyPrice);
            var difference = newTotalAmount - existingRental.TotalAmount;

            if (difference > 0)
            {
                var posResult = await _posService.PayAsync(rentalUpdateDto.CreditCardInformation, difference);
                if (!posResult.Success)
                {
                    throw new BusinessException(posResult.Message ?? "Ödeme sırasında bir hata oluştu, lütfen tekrar deneyin!");
                }

                var paymentDto = new PaymentCreateDto
                {
                    RentalId = existingRental.Id,
                    Amount = difference,
                    TransactionId = Guid.NewGuid().ToString(),
                    IsSuccessful = true
                };
                await _paymentService.AddAsync(paymentDto);

                existingRental.TotalAmount = newTotalAmount;
            }

            var customerResult = await _adminCustomerService.GetByIdAsync(existingRental.CustomerId);

            await SendRentalUpdateEmailAsync(
            customerResult.Data,
            carResult.Data,
            existingRental.TotalAmount,
            difference > 0 ? difference : 0, // Eğer difference 0'dan büyükse difference'ın kendisini kullan; DEĞİLSE 0 kullan.
            rentalUpdateDto.RentDate,
            rentalUpdateDto.ReturnDate
            );

            await _rentalRepository.UpdateAsync(existingRental);
            return new SuccessResult("Araç kiralama başarıyla güncellendi.");
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
