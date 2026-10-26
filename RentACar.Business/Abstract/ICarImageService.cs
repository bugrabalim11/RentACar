using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarImageDtos;

namespace RentACar.Business.Abstract
{
    public interface ICarImageService
    {
        Task<IDataResult<List<CarImageDetailDto>>> GetImagesByCarIdAsync(int carId);
    }
}
