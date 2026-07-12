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
        Task<List<BrandListDto>> GetAllAsync();
        Task<BrandListDto?> GetByIdAsync(int id);

        // Eski hali: Task AddAsync(BrandAddDto brandAddDto);
        Task<IResult> AddAsync(BrandAddDto brandAddDto);

        // Eski hali: Task<bool> UpdateAsync(BrandUpdateDto brandUpdateDto);
        Task<IResult> UpdateAsync(BrandUpdateDto brandUpdateDto);
        Task<bool> DeleteAsync(int id);

        // ... Delete ve GetAll gibi diğer metotları da ileride güncelleyeceğiz, şimdilik bu ikisine odaklanalım.
    }
}
