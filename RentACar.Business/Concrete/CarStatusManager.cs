using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    /// <summary>
    /// Bu sınıf, araçların anlık olarak kirada veya sanayide olma durumlarını kontrol ederek
    /// döngüsel bağımlılıkları (Circular Dependency) çözer.
    /// </summary>
    public class CarStatusManager : ICarStatusService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ICarMaintenanceRepository _carMaintenanceRepository;

        public CarStatusManager(IRentalRepository rentalRepository, ICarMaintenanceRepository carMaintenanceRepository)
        {
            _rentalRepository = rentalRepository;
            _carMaintenanceRepository = carMaintenanceRepository;
        }

        public async Task<IResult> CheckIfCarIsInMaintenanceAsync(int carId, DateTime startDate, DateTime? endDate)
        {
            // (Yeni Başlangıç <= Eski Bitiş) VE (Yeni Bitiş >= Eski Başlangıç)
            bool isMaintenance = await _carMaintenanceRepository.AnyAsync(x => x.CarId == carId && (x.CheckOutTime == null || startDate <= x.CheckOutTime) && (endDate == null || endDate >= x.CheckInTime));
            if (isMaintenance)
            {
                return new ErrorResult("Bu araç seçilen tarihlerde meşguldür!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfCarIsRentedAsync(int carId, DateTime startDate, DateTime? endDate)
        {
            bool isRented = await _rentalRepository.AnyAsync(x => x.CarId == carId && (x.ReturnDate == null || startDate <= x.ReturnDate) && (endDate == null || endDate >= x.RentDate));
            if (isRented)
            {
                return new ErrorResult("Bu araç seçilen tarihlerde meşguldür!");
            }
            return new SuccessResult();
        }
    }
}
