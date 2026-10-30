using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarDtos;

namespace RentACar.Business.Abstract
{
    public interface ICarService
    {
        Task<IDataResult<List<CarResultDto>>> GetCarsByColorIdAsync(int colorId);
        Task<IDataResult<List<CarResultDto>>> GetAllByBrandIdAsync(int brandId);
        Task<IDataResult<List<CarResultDto>>> GetAllAsync();
        Task<IDataResult<CarDetailDto>> GetByIdAsync(int id);
        Task<IResult> CheckIfCarExistsAsync(int carId);
        Task<IResult> CheckIfBrandIsUsedByAnyCarAsync(int brandId);
    }
}
