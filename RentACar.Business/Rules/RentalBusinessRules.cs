using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class RentalBusinessRules
    {
        private readonly ICarService _carService;
        private readonly ICustomerService _customerService;
        private readonly IFindexScoreService _findexScoreService;
        private readonly ICarStatusService _carStatusService;
        private readonly IRentalRepository _rentalRepository;
        private readonly IAdminCustomerService _adminCustomerService;

        public RentalBusinessRules(ICarService carService, ICustomerService customerService, IFindexScoreService findexScoreService, ICarStatusService carStatusService, IRentalRepository rentalRepository,IAdminCustomerService adminCustomerService)
        {
            _carService = carService;
            _customerService = customerService;
            _findexScoreService = findexScoreService;
            _carStatusService = carStatusService;
            _rentalRepository = rentalRepository;
            _adminCustomerService = adminCustomerService;
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

        public async Task<IResult> CheckIfCustomerDrivingExperienceIsSufficient(int carId, int customerId)
        {
            var carResult = await _carService.GetByIdAsync(carId);
            var customerResult = await _adminCustomerService.GetByIdAsync(customerId);

            int customerExperience = DateTime.UtcNow.Year - customerResult.Data.DrivingLicenseYear;
            if (customerExperience < carResult.Data.MinDrivingExperience)
            {
                return new ErrorResult("Bu aracı kiralayabilmek için ehliyet süreniz yetersizdir!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfCustomerFindexScoreIsSufficient(int carId, int customerId)
        {
            var carResult = await _carService.GetByIdAsync(carId);
            var customerResult = await _adminCustomerService.GetByIdAsync(customerId);

            int findexResult = _findexScoreService.GetScoreByCustomerId(customerId);
            if (findexResult < carResult.Data.MinFindexScore)
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
