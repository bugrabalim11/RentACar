using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.RentalDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class RentalManager : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        private readonly ICarService _carService;
        private readonly ICustomerService _customerService;
        private readonly IPaymentService _paymentService;
        private readonly ICarStatusService _carStatusService;
        private readonly IFindexScoreService _findexScoreService;
        public RentalManager(IRentalRepository rentalRepository, IMapper mapper, ICarService carService, ICustomerService customerService, IPaymentService paymentService, ICarStatusService carStatusService, IFindexScoreService findexScoreService)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _carService = carService;
            _customerService = customerService;
            _paymentService = paymentService;
            _carStatusService = carStatusService;
            _findexScoreService = findexScoreService;
        }

        public async Task<IResult> AddAsync(RentalAddDto rentalAddDto, int userId)
        {
            // 1. RentDate (Başlangıç tarihi) zaten boş olamaz (Nullable değil). Ona direkt etiketi bas:
            rentalAddDto.RentDate = DateTime.SpecifyKind(rentalAddDto.RentDate, DateTimeKind.Utc);
            // 2. ReturnDate (Bitiş tarihi) bir kutu (Nullable). Kutuyu salla:
            if (rentalAddDto.ReturnDate.HasValue)
            {
                // Kutu doluysa: Kutunun içindeki SAATİ (.Value) al, ona UTC etiketini bas 
                // ve kutunun içine yeni UTC'li haliyle geri koy!
                rentalAddDto.ReturnDate = DateTime.SpecifyKind(rentalAddDto.ReturnDate.Value, DateTimeKind.Utc);
            }

            // 1. ADIM: VATANDAŞI (User) MÜŞTERİYE (Customer) ÇEVİRME
            // Token'dan sadece UserId (Vatandaş Kimliği) geliyor. Ancak kiralama tablosu (Rental)
            // işlemleri CustomerId (Müşteri Dosyası) üzerinden yapar.
            // Bu yüzden UserId ile veritabanına gidip adamın Müşteri Profilini (Dosyasını) buluyoruz.
            var customerResult = await _customerService.GetMyCustomerProfileAsync(userId);
            if (!customerResult.Success)
            {
                return new ErrorResult("Kiralama yapabilmek için lütfen ilk önce müşteri profilinizi oluşturun!");
            }

            IResult? result = BusinessRules.Run(
            CheckIfRentDateBeforeToday(rentalAddDto.RentDate),
            await _carService.CheckIfCarExistsAsync(rentalAddDto.CarId),
            await _carStatusService.CheckIfCarIsInMaintenanceAsync(rentalAddDto.CarId, rentalAddDto.RentDate, rentalAddDto.ReturnDate),
            await CheckIfCustomerDrivingExperienceIsSufficient(rentalAddDto.CarId, customerResult.Data.Id),
            await CheckIfCarAvailable(rentalAddDto.CarId, rentalAddDto.RentDate, rentalAddDto.ReturnDate)
            );
            if (result != null)
            {
                return result;
            }

            var rental = _mapper.Map<Rental>(rentalAddDto);
            // 2. ADIM: DOSYA NUMARASINI ZIMBALAMA
            // Arşiv memurunun bize getirdiği dosyanın içindeki Müşteri Numarasını (Data.Id),
            // yeni kiralama faturamızın (rental) üzerine kalıcı olarak zımbalıyoruz.
            rental.CustomerId = customerResult.Data.Id;

            var car = await _carService.GetByIdAsync(rental.CarId);
            int totalDays = 1;
            if (rental.ReturnDate.HasValue)
            {
                var timeSpan = rental.ReturnDate.Value - rental.RentDate;
                totalDays = timeSpan.Days;

                // Aynı gün getirirse 0 çıkmasın diye senin o harika kalkanını buraya da koyalım:
                if (totalDays == 0) totalDays = 1;
            }
            decimal totalAmount = totalDays * car.Data.DailyPrice;
            var paymentResult = await _paymentService.PayAsync(rentalAddDto.CreditCardInformation, totalAmount);
            if (!paymentResult.Success)
            {
                return new ErrorResult(paymentResult.Message ?? "Ödeme sırasında bir hata oluştu, lütfen tekrar deneyin!");
            }

            await _rentalRepository.AddAsync(rental);
            return new SuccessResult("Araç kiralama başarıyla oluşturuldu.");
        }

        public async Task<IResult> AddByAdminAsync(RentalAddByAdminDto rentalAddByAdminDto)
        {
            rentalAddByAdminDto.RentDate = DateTime.SpecifyKind(rentalAddByAdminDto.RentDate, DateTimeKind.Utc);
            if (rentalAddByAdminDto.ReturnDate.HasValue)
            {
                rentalAddByAdminDto.ReturnDate = DateTime.SpecifyKind(rentalAddByAdminDto.ReturnDate.Value, DateTimeKind.Utc);
            }

            IResult? result = BusinessRules.Run(
            CheckIfRentDateBeforeToday(rentalAddByAdminDto.RentDate),
            await _customerService.CheckIfCustomerExistsByIdAsync(rentalAddByAdminDto.CustomerId),
            await _carService.CheckIfCarExistsAsync(rentalAddByAdminDto.CarId),
            await _carStatusService.CheckIfCarIsInMaintenanceAsync(rentalAddByAdminDto.CarId, rentalAddByAdminDto.RentDate, rentalAddByAdminDto.ReturnDate),
            await CheckIfCustomerDrivingExperienceIsSufficient(rentalAddByAdminDto.CarId, rentalAddByAdminDto.CustomerId),
            await CheckIfCarAvailable(rentalAddByAdminDto.CarId, rentalAddByAdminDto.RentDate, rentalAddByAdminDto.ReturnDate)
            );
            if (result != null)
            {
                return result;
            }

            var rental = _mapper.Map<Rental>(rentalAddByAdminDto);

            var car = await _carService.GetByIdAsync(rental.CarId);
            int totalDays = 1;
            if (rental.ReturnDate.HasValue)
            {
                var timeSpan = rental.ReturnDate.Value - rental.RentDate;
                totalDays = timeSpan.Days;
                if (totalDays == 0) totalDays = 1;
            }
            decimal totalAmount = totalDays * car.Data.DailyPrice;
            var paymentResult = await _paymentService.PayAsync(rentalAddByAdminDto.CreditCardInformation, totalAmount);
            if (!paymentResult.Success)
            {
                return new ErrorResult(paymentResult.Message ?? "Ödeme sırasında bir hata oluştu, lütfen tekrar deneyin!");
            }

            await _rentalRepository.AddAsync(rental);
            return new SuccessResult("Araç kiralama başarıyla oluşturuldu.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingRental = await _rentalRepository.GetAsync(x => x.Id == id);
            if (existingRental == null)
            {
                return new ErrorResult("Silinecek araç kiralama bulunamadı.");
            }

            existingRental.IsDeleted = true;
            existingRental.DeletedDate = DateTime.UtcNow;
            await _rentalRepository.UpdateAsync(existingRental);
            return new SuccessResult("Araç kiralama başarıyla silindi.");
        }

        public async Task<IDataResult<List<RentalListDto>>> GetAllAsync()
        {
            var rentals = await _rentalRepository.GetRentalsWithDetailsAsync();
            var rentalsListDtos = _mapper.Map<List<RentalListDto>>(rentals);
            return new SuccessDataResult<List<RentalListDto>>(rentalsListDtos, "Kiralama işlemleri başarıyla listelendi.");
        }

        public async Task<IDataResult<List<RentalListDto>>> GetAllByUserIdAsync(int userId)
        {
            var rentals = await _rentalRepository.GetRentalsByUserIdAsync(userId);
            if (rentals == null || !rentals.Any())
            {
                return new ErrorDataResult<List<RentalListDto>>("Kullanıcıya ait kiralama işlemleri bulunamadı.");
            }

            var mappedRentals = _mapper.Map<List<RentalListDto>>(rentals);
            return new SuccessDataResult<List<RentalListDto>>(mappedRentals, "Kullanıcıya ait kiralama işlemleri başarıyla listelendi.");
        }

        public async Task<IDataResult<RentalDetailDto>> GetMyRentalByIdAsync(int rentalId, int userId)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(rentalId);
            if (rental == null)
            {
                return new ErrorDataResult<RentalDetailDto>("Aradağınız kiralama bulunamadı!");
            }

            if (rental.Customer.UserId != userId)
            {
                return new ErrorDataResult<RentalDetailDto>("Güvenlik İhlali: Bu kiralama kaydını (faturayı) görüntüleme yetkiniz yok!");
            }

            var mappedRental = _mapper.Map<RentalDetailDto>(rental);
            return new SuccessDataResult<RentalDetailDto>(mappedRental, "Kiralama detaylarınız başarıyla getirildi.");
        }

        public async Task<IDataResult<RentalDetailDto>> GetByIdAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(id);
            if (rental == null)
            {
                return new ErrorDataResult<RentalDetailDto>("Aranan araç kiralama bulunamadı.");
            }

            var rentalDetailDto = _mapper.Map<RentalDetailDto>(rental);
            return new SuccessDataResult<RentalDetailDto>(rentalDetailDto, "Araç kiralama detayı getirildi.");
        }

        public async Task<IResult> UpdateAsync(RentalUpdateDto rentalUpdateDto)
        {
            var existingRental = await _rentalRepository.GetAsync(x => x.Id == rentalUpdateDto.Id);
            if (existingRental == null)
            {
                return new ErrorResult("Güncellenecek araç kiralama bulunamadı.");
            }

            rentalUpdateDto.RentDate = rentalUpdateDto.RentDate.ToUniversalTime();
            if (rentalUpdateDto.ReturnDate.HasValue)
            {
                rentalUpdateDto.ReturnDate = rentalUpdateDto.ReturnDate.Value.ToUniversalTime();
            }

            IResult? result = BusinessRules.Run(
            CheckIfRentalIsAlreadyCompleted(existingRental.ReturnDate),
            CheckIfRentDateBeforeToday(rentalUpdateDto.RentDate),
            await _carService.CheckIfCarExistsAsync(rentalUpdateDto.CarId),
            await _carStatusService.CheckIfCarIsInMaintenanceAsync(rentalUpdateDto.CarId, rentalUpdateDto.RentDate, rentalUpdateDto.ReturnDate),
            await CheckIfCustomerDrivingExperienceIsSufficient(rentalUpdateDto.CarId, existingRental.CustomerId),
            await CheckIfCarAvailableForUpdate(rentalUpdateDto.Id, rentalUpdateDto.CarId, rentalUpdateDto.RentDate, rentalUpdateDto.ReturnDate)
            );
            if (result != null)
            {
                return result;
            }

            _mapper.Map(rentalUpdateDto, existingRental);
            await _rentalRepository.UpdateAsync(existingRental);
            return new SuccessResult("Araç kiralama başarıyla güncellendi.");
        }

        public async Task<IResult> UpdateMyRentalAsync(int userId, int rentalId, RentalUpdateReturnDateDto rentalUpdateReturnDateDto)
        {
            rentalUpdateReturnDateDto.ReturnDate = DateTime.SpecifyKind(rentalUpdateReturnDateDto.ReturnDate, DateTimeKind.Utc);

            var existingRental = await _rentalRepository.GetRentalWithDetailsByIdAsync(rentalId);
            if (existingRental == null) return new ErrorResult("Kiralama bulunamadı!");

            if (existingRental.Customer.UserId != userId)
            {
                return new ErrorResult("Bu kiralamayı güncellemeye yetkiniz yok!");
            }

            IResult? result = BusinessRules.Run
            (
                CheckIfRentalIsAlreadyCompleted(existingRental.ReturnDate),
                CheckIfReturnDateIsAfterRentDate(existingRental.RentDate, rentalUpdateReturnDateDto.ReturnDate),
                await CheckIfCarAvailableForUpdate(rentalId, existingRental.CarId, existingRental.RentDate, rentalUpdateReturnDateDto.ReturnDate)
            );
            if (result != null)
            {
                return result;
            }

            existingRental.ReturnDate = rentalUpdateReturnDateDto.ReturnDate;
            await _rentalRepository.UpdateAsync(existingRental);
            return new SuccessResult("Araç teslim tarihiniz başarıyla güncellendi.");
        }

        public async Task<IResult> CheckIfAnyRentalExistsByOfficeIdAsync(int officeId)
        {
            bool result = await _rentalRepository.AnyAsync(x => x.PickUpOfficeId == officeId || x.DropOffOfficeId == officeId);
            if (result)
            {
                return new ErrorResult("Ofise ait kiralama işlemleri mevcut, bu yüzden silinemez!");
            }
            return new SuccessResult();
        }

        // Bu metot sadece bu sınıfın (Manager'ın) içinde kullanılacağı için 'private' yapıyoruz.
        // Amacımız: Verilen araba ID'si, istenen tarihler arasında başka bir kiralama kaydında var mı?
        private async Task<IResult> CheckIfCarAvailable(int carId, DateTime rentDate, DateTime? returnDate)
        {
            bool isExist = await _rentalRepository
                .AnyAsync(x => x.CarId == carId && (x.ReturnDate == null || rentDate <= x.ReturnDate) && (returnDate == null || returnDate >= x.RentDate));
            if (isExist)
            {
                return new ErrorResult("Lütfen geçerli tarih ve araç seçiniz.");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfCarAvailableForUpdate(int rentalId, int carId, DateTime rentDate, DateTime? returnDate)
        {
            bool isExist = await _rentalRepository
               .AnyAsync(x => x.CarId == carId && (x.Id != rentalId) && (x.ReturnDate == null || rentDate <= x.ReturnDate) && (returnDate == null || returnDate >= x.RentDate));
            if (isExist)
            {
                // Kullanıcıya tam olarak neden reddedildiğini açıklayan net bir mesaj
                return new ErrorResult("Bu araç, güncellemek istediğiniz tarihler arasında başka bir müşteriye kiralanmıştır.");
            }
            // Alt kural olduğu için sadece "Geçiş İzni" veriyoruz, tebrik mesajına gerek yok.
            return new SuccessResult();
        }

        // .Date dediğimizde saati çöpe atar sadece tarihe bakar
        // Veritabanı sorgusu yapmadığımız için senkron olarak kodladık
        private IResult CheckIfRentDateBeforeToday(DateTime rentDate)
        {
            if (rentDate.Date < DateTime.UtcNow.Date)
            {
                return new ErrorResult("Geçmiş bir tarihe kiralama yapılamaz!");
            }
            return new SuccessResult();
        }

        private IResult CheckIfReturnDateIsAfterRentDate(DateTime rentDate, DateTime returnDate)
        {
            if (returnDate.Date < rentDate.Date)
            {
                return new ErrorResult("Dönüş tarihi, kiralama başlangıç tarihinden önce olamaz!");
            }
            return new SuccessResult();
        }

        private IResult CheckIfRentalIsAlreadyCompleted(DateTime? returnDate)
        {
            // Kural 1: Araç henüz teslim edilmemiş (ucu açık kiralama). Güncellemeye izin ver.
            if (returnDate == null)
            {
                return new SuccessResult();
            }

            // Kural 2: Dönüş tarihi UTC olarak şu andan küçükse, bu dosya arşive kalkmıştır.
            // Not: returnDate nullable (DateTime?) olduğu için, içindeki salt tarihe ulaşmak zorundayız. 
            // Yukarıda null kontrolünü geçtiğimiz için burada gönül rahatlığıyla .Value diyerek içindeki tarihi çekebiliyoruz.
            if (returnDate.Value < DateTime.UtcNow)
            {
                return new ErrorResult("Sona ermiş veya geçmişteki bir kiralama kaydını güncelleyemezsiniz!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfCustomerDrivingExperienceIsSufficient(int carId, int customerId)
        {
            var carResult = await _carService.GetByIdAsync(carId);
            if (!carResult.Success)
            {
                return new ErrorResult("Araç bilgileri bulunamadı!");
            }
            var customerResult = await _customerService.GetByIdAsync(customerId);
            if (!customerResult.Success)
            {
                return new ErrorResult("Müşteri bilgileri bulunamadı!");
            }

            int customerExperience = DateTime.UtcNow.Year - customerResult.Data.DrivingLicenseYear;
            if (customerExperience < carResult.Data.MinDrivingExperience)
            {
                return new ErrorResult("Bu aracı kiralayabilmek için ehliyet süreniz yetersizdir!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfCustomerFindexScoreIsSufficient(int carId, int customerId)
        {
            var carResult = await _carService.GetByIdAsync(carId);
            if (!carResult.Success)
            {
                return new ErrorResult("Araç bulunamadı!");
            }

            var customerResult = await _customerService.GetByIdAsync(customerId);
            if (!customerResult.Success)
            {
                return new ErrorResult("Müşteri bulunamadı!");
            }

            int findexResult = _findexScoreService.GetScoreByCustomerId(customerId);
            if (findexResult < carResult.Data.MinFindexScore)
            {
                return new ErrorResult("Bu aracı kiralamaya findex puanınız yetmiyor!");
            }
            return new SuccessResult();
        }
    }
}
