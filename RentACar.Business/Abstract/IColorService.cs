using RentACar.Core.Utilities.Results;
using RentACar.Dtos.ColorDtos;

namespace RentACar.Business.Abstract
{
    public interface IColorService
    {
        Task<IDataResult<List<ColorResultDto>>> GetAllAsync();
        Task<IDataResult<ColorResultDto>> GetByIdAsync(int id);
        Task<IDataResult<int>> AddAsync(ColorCreateDto colorAddDto);
        Task<IResult> UpdateAsync(ColorUpdateDto colorUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
