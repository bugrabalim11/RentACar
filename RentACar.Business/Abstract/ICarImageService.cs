using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarImageDtos;
using IResult = RentACar.Core.Utilities.Results.IResult;

namespace RentACar.Business.Abstract
{
    public interface ICarImageService
    {
        Task<IDataResult<List<CarImageDetailDto>>> GetAllAsync();
        Task<IDataResult<CarImageDetailDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(CarImageAddDto carImageAddDto);
        Task<IResult> UpdateAsync(CarImageUpdateDto carImageUpdateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<List<CarImageDetailDto>>> GetImagesByCarIdAsync(int carId);
    }
}
