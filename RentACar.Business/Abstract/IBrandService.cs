using RentACar.Core.Utilities.Results;
using RentACar.Dtos.BrandDtos;

namespace RentACar.Business.Abstract
{
    public interface IBrandService
    {
        Task<IDataResult<List<BrandResultDto>>> GetAllAsync();
        Task<IDataResult<BrandResultDto>> GetByIdAsync(int id);
    }
}
