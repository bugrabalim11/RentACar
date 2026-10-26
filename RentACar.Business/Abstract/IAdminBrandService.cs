using RentACar.Core.Utilities.Results;
using RentACar.Dtos.BrandDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminBrandService
    {
        Task<IDataResult<int>> AddAsync(BrandCreateDto brandAddDto);
        Task<IResult> UpdateAsync(BrandUpdateDto brandUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
