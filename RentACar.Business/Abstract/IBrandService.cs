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
        Task AddAsync(BrandAddDto brandAddDto);
        Task<bool> UpdateAsync(BrandUpdateDto brandUpdateDto);
        Task<bool> DeleteAsync(int id);
    }
}
