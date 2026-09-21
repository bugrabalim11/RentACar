using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarDtos;

namespace RentACar.Business.Abstract
{
    public interface ICarService
    {
        Task<IDataResult<List<CarResultDto>>> GetCarsByColorIdAsync(int colorId);
        Task<IDataResult<List<CarResultDto>>> GetAllByBrandIdAsync(int brandId);

        // 1. Liste Dönerken (Join'li veriler bu kutuya girecek)
        Task<IDataResult<List<CarResultDto>>> GetAllAsync();

        // 2. Tekil Dönerken
        Task<IDataResult<CarDetailDto>> GetByIdAsync(int id);

        // 3. Ekle, Sil, Güncelle işlemleri sadece boş kargo kutusu (IResult) döner
        Task<IResult> AddAsync(CarCreateDto carAddDto);
        Task<IResult> UpdateAsync(CarUpdateDto carUpdateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> CheckIfCarExistsAsync(int carId);
    }
}
