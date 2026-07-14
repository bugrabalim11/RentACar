using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CustomerDtos;
using RentACar.Dtos.OfficeDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IOfficeService
    {
        Task<IDataResult<List<OfficeListDto>>> GetAllAsync();
        Task<IDataResult<OfficeListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(OfficeAddDto officeAddDto);
        Task<IResult> UpdateAsync(OfficeUpdateDto officeUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
