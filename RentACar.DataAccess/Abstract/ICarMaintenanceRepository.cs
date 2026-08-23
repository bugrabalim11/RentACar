using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Abstract
{
    public interface ICarMaintenanceRepository : IRepository<CarMaintenance>
    {
        Task<List<CarMaintenance>> GetCarMaintenanceWithDetailsAsync();
        Task<CarMaintenance?> GetCarMaintenanceByIdWithDetailsAsync(int id);
    }
}
