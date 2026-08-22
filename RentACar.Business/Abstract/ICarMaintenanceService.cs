using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.Business.Abstract
{
    public interface ICarMaintenanceService
    {
        Task<IDataResult<List<CarMaintenanceListDto>>> GetAllAsync();
        Task<IDataResult<CarMaintenanceListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(CarMaintenanceAddDto carMaintenanceAddDto);
        Task<IResult> UpdateAsync(CarMaintenanceUpdateDto carMaintenanceUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
