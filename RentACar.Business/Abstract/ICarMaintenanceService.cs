using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.Business.Abstract
{
    public interface ICarMaintenanceService
    {
        Task<IDataResult<List<CarMaintenanceResultDto>>> GetAllAsync();
        Task<IDataResult<CarMaintenanceResultDto>> GetByIdAsync(int id);
    }
}
