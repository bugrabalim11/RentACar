using RentACar.Core.Utilities.Results;
using RentACar.Dtos.OfficeDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminOfficeService
    {
        Task<IDataResult<int>> AddAsync(OfficeCreateDto officeAddDto);
        Task<IResult> UpdateAsync(OfficeUpdateDto officeUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
