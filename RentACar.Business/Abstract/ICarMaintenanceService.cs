using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.Business.Abstract
{
    public interface ICarMaintenanceService
    {
        Task<IDataResult<List<CarMaintenanceResultDto>>> GetAllAsync();
        Task<IDataResult<CarMaintenanceResultDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(CarMaintenanceCreateDto carMaintenanceAddDto);
        Task<IResult> UpdateAsync(CarMaintenanceUpdateDto carMaintenanceUpdateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> CheckIfCarAvailableForMaintenance(int carId, DateTime checkInTime, DateTime? checkOutTime);
    }
}
