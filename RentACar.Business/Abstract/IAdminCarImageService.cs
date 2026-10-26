using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarImageDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminCarImageService
    {
        Task<IDataResult<int>> AddAsync(CarImageCreateDto carImageAddDto);
        Task<IResult> UpdateAsync(CarImageUpdateDto carImageUpdateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> DeleteOldImagesAsync();
        Task<IResult> RestoreAsync(int id);
        Task<IDataResult<List<CarImageDeletedDto>>> GetDeletedImagesByCarIdAsync(int carId);
    }
}
