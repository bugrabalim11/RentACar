using RentACar.Core.Utilities.Results;
using RentACar.Dtos.BrandDtos;
using RentACar.Dtos.ColorDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IColorService
    {
        Task<IDataResult<List<ColorListDto>>> GetAllAsync();
        Task<IDataResult<ColorListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(ColorAddDto colorAddDto);
        Task<IResult> UpdateAsync(ColorUpdateDto colorUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
