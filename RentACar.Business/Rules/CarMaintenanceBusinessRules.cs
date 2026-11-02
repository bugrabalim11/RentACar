using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class CarMaintenanceBusinessRules
    {
        private readonly ICarMaintenanceRepository _carMaintenanceRepository;
        private readonly ICarService _carService;
        private readonly ICarStatusService _carStatusService;

        public CarMaintenanceBusinessRules(ICarMaintenanceRepository carMaintenanceRepository, ICarService carService, ICarStatusService carStatusService)
        {
            _carMaintenanceRepository = carMaintenanceRepository;
            _carService = carService;
            _carStatusService = carStatusService;
        }

        public async Task<IResult> CheckIfCarAvailableForMaintenance(int carId, DateTime checkInTime, DateTime? checkOutTime)
        {
            bool isExist = await _carMaintenanceRepository.AnyAsync(x => x.CarId == carId && (x.CheckOutTime == null || checkInTime <= x.CheckOutTime) && (checkOutTime == null || checkOutTime >= x.CheckInTime));
            if (isExist)
            {
                return new ErrorResult("Bu araç, seçilen tarihler arasında zaten sanayidedir!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfCarAvailableForMaintenanceForUpdate(int maintenanceId, int carId, DateTime checkInTime, DateTime? checkOutTime)
        {
            bool isExist = await _carMaintenanceRepository.AnyAsync(x => x.CarId == carId && (x.Id != maintenanceId) && (x.CheckOutTime == null || checkInTime <= x.CheckOutTime) && (checkOutTime == null || checkOutTime >= x.CheckInTime));
            if (isExist)
            {
                return new ErrorResult("Bu araç, güncellenmek istenen tarihler arasında zaten sanayidedir!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfCarExists(int carId)
        {
            return await _carService.CheckIfCarExistsAsync(carId);
        }

        public async Task<IResult> CheckIfCarIsRentedAsync(int carId, DateTime startDate, DateTime? endDate)
        {
           return await _carStatusService.CheckIfCarIsRentedAsync(carId, startDate, endDate);
        }
    }
}
