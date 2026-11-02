using RentACar.Core.Utilities.Results;
using RentACar.Dtos.OfficeDtos;

namespace RentACar.Business.Abstract
{
    public interface IOfficeService
    {
        Task<IDataResult<List<OfficeResultDto>>> GetAllAsync();
        Task<IDataResult<OfficeResultDto>> GetByIdAsync(int id);
    }
}
