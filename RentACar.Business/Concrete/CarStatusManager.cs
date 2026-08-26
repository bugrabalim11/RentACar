using RentACar.Business.Abstract;
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
    }
}
