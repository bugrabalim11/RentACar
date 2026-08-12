using RentACar.Core.Utilities.Results;
using RentACar.Dtos.OfficeDtos;

namespace RentACar.Business.Abstract
{
    public interface IOfficeService
    {
        Task<IDataResult<List<OfficeListDto>>> GetAllAsync();
        Task<IDataResult<OfficeListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(OfficeAddDto officeAddDto);
        Task<IResult> UpdateAsync(OfficeUpdateDto officeUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
