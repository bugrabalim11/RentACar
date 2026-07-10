using RentACar.Dtos.BrandDtos;
using RentACar.Dtos.ColorDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IColorService
    {
        Task<List<ColorListDto>> GetAllAsync();
        Task<ColorListDto?> GetByIdAsync(int id);
        Task AddAsync(ColorAddDto colorAddDto);
        Task<bool> UpdateAsync(ColorUpdateDto colorUpdateDto);
        Task<bool> DeleteAsync(int id);
    }
}
