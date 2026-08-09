using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarDtos;

namespace RentACar.Business.Abstract
{
    public interface ICarService
    {
        Task<IDataResult<List<CarListDto>>> GetAllByBrandIdAsync(int brandId);

        // 1. Liste Dönerken (Join'li veriler bu kutuya girecek)
        Task<IDataResult<List<CarListDto>>> GetAllAsync();

        // 2. Tekil Dönerken
        Task<IDataResult<CarDetailDto>> GetByIdAsync(int id);

        // 3. Ekle, Sil, Güncelle işlemleri sadece boş kargo kutusu (IResult) döner
        Task<IResult> AddAsync(CarAddDto carAddDto);
        Task<IResult> UpdateAsync(CarUpdateDto carUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
