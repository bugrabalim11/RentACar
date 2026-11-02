using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminCarMaintenanceService
    {
        Task<IDataResult<int>> AddAsync(CarMaintenanceCreateDto carMaintenanceAddDto);
        Task<IResult> UpdateAsync(CarMaintenanceUpdateDto carMaintenanceUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
