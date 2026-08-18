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
        public RentalManager(IRentalRepository rentalRepository, IMapper mapper, ICarService carService, ICustomerService customerService)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _carService = carService;
            _customerService = customerService;
        }

        public async Task<IResult> AddAsync(RentalAddDto rentalAddDto, int userId)
        {
            // --- POSTGRESQL ZAMAN DİLİMİ KURALI (UTC) ---
            // PostgreSQL, saat dilimi belirtilmemiş (Kind=Unspecified) tarihleri kabul etmez.
            // Frontend veya Swagger'dan gelen saf tarihleri veritabanı deposuna göndermeden önce,
            // Gümrük kurallarına uymak adına Evrensel Saate (UTC) dönüştürerek standartlaştırıyoruz.
            rentalAddDto.RentDate = rentalAddDto.RentDate.ToUniversalTime();
            if (rentalAddDto.ReturnDate.HasValue)
            {
                rentalAddDto.ReturnDate = rentalAddDto.ReturnDate.Value.ToUniversalTime();
            }

            var customerResult = await _customerService.GetMyCustomerProfileAsync(userId);
            if (!customerResult.Success)
            {
                return new ErrorResult("Kiralama yapabilmek için lütfen ilk önce müşteri profilinizi oluşturun!");
            }

            IResult? result = BusinessRules.Run
            (
            await CheckIfCarAvailable(rentalAddDto.CarId, rentalAddDto.RentDate, rentalAddDto.ReturnDate),
            await _carService.CheckIfCarExistsAsync(rentalAddDto.CarId),
            CheckIfRentDateBeforeToday(rentalAddDto.RentDate)
            );
            if (result != null)
            {
                return result;
            }

            var rental = _mapper.Map<Rental>(rentalAddDto);
            rental.CustomerId = customerResult.Data.Id;
            await _rentalRepository.AddAsync(rental);
            return new SuccessResult("Araç kiralama başarıyla eklendi.");
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

        public async Task<IDataResult<RentalListDto>> GetMyRentalByIdAsync(int rentalId, int userId)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(rentalId);
            if (rental == null)
            {
                return new ErrorDataResult<RentalListDto>("Aradağınız kiralama bulunamadı!");
            }

            if (rental.Customer.UserId != userId)
            {
                return new ErrorDataResult<RentalListDto>("Güvenlik İhlali: Bu kiralama kaydını (faturayı) görüntüleme yetkiniz yok!");
            }

            var mappedRental = _mapper.Map<RentalListDto>(rental);
            return new SuccessDataResult<RentalListDto>(mappedRental, "Kiralama detaylarınız başarıyla getirildi.");
        }

        public async Task<IDataResult<RentalListDto>> GetByIdAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(id);
            if (rental == null)
            {
                return new ErrorDataResult<RentalListDto>("Aranan araç kiralama bulunamadı.");
            }

            var rentalListDto = _mapper.Map<RentalListDto>(rental);
            return new SuccessDataResult<RentalListDto>(rentalListDto, "Araç kiralama detayı getirildi.");
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

            IResult? result = BusinessRules.Run
            (
                CheckIfRentalIsAlreadyCompleted(existingRental.ReturnDate),
                await CheckIfCarAvailableForUpdate(rentalUpdateDto.Id, rentalUpdateDto.CarId, rentalUpdateDto.RentDate, rentalUpdateDto.ReturnDate),
                await _carService.CheckIfCarExistsAsync(rentalUpdateDto.CarId),
                CheckIfRentDateBeforeToday(rentalUpdateDto.RentDate)
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
    }
}
