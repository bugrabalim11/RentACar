using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class RentalBusinessRules
    {
        private readonly ICarService _carService;
        private readonly ICustomerService _customerService;
        private readonly ICarStatusService _carStatusService;
        private readonly IRentalRepository _rentalRepository;

        public RentalBusinessRules(ICarService carService, ICustomerService customerService, ICarStatusService carStatusService, IRentalRepository rentalRepository)
        {
            _carService = carService;
            _customerService = customerService;
            _carStatusService = carStatusService;
            _rentalRepository = rentalRepository;
        }

        public async Task<IResult> CheckIfCustomerExistsAsync(int customerId)
        {
            return await _customerService.CheckIfCustomerExistsByIdAsync(customerId);
        }

        public async Task<IResult> CheckIfCarExistsAsync(int carId)
        {
            return await _carService.CheckIfCarExistsAsync(carId);
        }

        public async Task<IResult> CheckIfCarIsInMaintenanceAsync(int carId, DateTime startDate, DateTime? endDate)
        {
            return await _carStatusService.CheckIfCarIsInMaintenanceAsync(carId, startDate, endDate);
        }

        public IResult CheckIfCustomerDrivingExperienceIsSufficient(int minDrivingExperience, int drivingLicenseYear)
        {
            int experience = DateTime.UtcNow.Year - drivingLicenseYear;
            if (experience < minDrivingExperience)
            {
                return new ErrorResult("Bu aracı kiralayabilmek için ehliyet süreniz yetersizdir!");
            }
            return new SuccessResult();
        }

        public IResult CheckIfCustomerFindexScoreIsSufficient(int minFindexScore, int customerFindexScore)
        {
            if (customerFindexScore < minFindexScore)
            {
                return new ErrorResult("Bu aracı kiralamaya findex puanınız yetmiyor!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfCarAvailableForUpdate(int rentalId, int carId, DateTime rentDate, DateTime? returnDate)
        {
            bool isExist = await _rentalRepository.AnyAsync(x => x.CarId == carId && (x.Id != rentalId) && (x.ReturnDate == null || rentDate <= x.ReturnDate) && (returnDate == null || returnDate >= x.RentDate));
            if (isExist)
            {
                return new ErrorResult("Bu araç, güncellemek istediğiniz tarihler arasında başka bir müşteriye kiralanmıştır.");
            }
            return new SuccessResult();
        }

        public IResult CheckIfRentDateBeforeToday(DateTime rentDate)
        {
            if (rentDate.Date < DateTime.UtcNow.Date)
            {
                return new ErrorResult("Geçmiş bir tarihe kiralama yapılamaz!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfCarAvailable(int carId, DateTime rentDate, DateTime? returnDate)
        {
            bool isExist = await _rentalRepository.AnyAsync(x => x.CarId == carId && (x.ReturnDate == null || rentDate <= x.ReturnDate) && (returnDate == null || returnDate >= x.RentDate));
            if (isExist)
            {
                return new ErrorResult("Lütfen geçerli tarih ve araç seçiniz.");
            }
            return new SuccessResult();
        }

        public IResult CheckIfRentalIsAlreadyCompleted(DateTime? returnDate)
        {
            if (returnDate == null)
            {
                return new SuccessResult();
            }
            if (returnDate.Value < DateTime.UtcNow)
            {
                return new ErrorResult("Sona ermiş veya geçmişteki bir kiralama kaydını güncelleyemezsiniz!");
            }
            return new SuccessResult();
        }
    }
}
