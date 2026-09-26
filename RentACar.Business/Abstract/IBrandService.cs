using RentACar.Core.Utilities.Results;
using RentACar.Dtos.BrandDtos;

namespace RentACar.Business.Abstract
{
    public interface IBrandService
    {
        // Liste döneceğimiz için IDataResult içine List<Brand> koyuyoruz
        Task<IDataResult<List<BrandResultDto>>> GetAllAsync();

        // Tek bir marka döneceğimiz için IDataResult içine tek bir Brand koyuyoruz
        Task<IDataResult<BrandResultDto>> GetByIdAsync(int id);

        // Eski hali: Task AddAsync(BrandCreateDto brandAddDto);
        Task<IDataResult<int>> AddAsync(BrandCreateDto brandAddDto);

        // Eski hali: Task<bool> UpdateAsync(BrandUpdateDto brandUpdateDto);
        Task<IResult> UpdateAsync(BrandUpdateDto brandUpdateDto);

        // Silme işlemi sadece başarı/başarısızlık döner
        Task<IResult> DeleteAsync(int id);
    }
}
