using RentACar.Core.Utilities.Results;
using RentACar.Dtos.BrandDtos;
using RentACar.Dtos.CarDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IBrandService
    {
        // Liste döneceğimiz için IDataResult içine List<Brand> koyuyoruz
        Task<IDataResult<List<BrandListDto>>> GetAllAsync();

        // Tek bir marka döneceğimiz için IDataResult içine tek bir Brand koyuyoruz
        Task<IDataResult<BrandListDto>> GetByIdAsync(int id);

        // Eski hali: Task AddAsync(BrandAddDto brandAddDto);
        Task<IResult> AddAsync(BrandAddDto brandAddDto);

        // Eski hali: Task<bool> UpdateAsync(BrandUpdateDto brandUpdateDto);
        Task<IResult> UpdateAsync(BrandUpdateDto brandUpdateDto);

        // Silme işlemi sadece başarı/başarısızlık döner
        Task<IResult> DeleteAsync(int id);
    }
}
