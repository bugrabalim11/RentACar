using RentACar.Core.Utilities.Results;
using RentACar.Dtos.ContactInfoDtos;
using RentACar.Dtos.OfficeDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IContactInfoService
    {
        Task<IDataResult<List<ContactInfoResultDto>>> GetAllAsync();
        Task<IDataResult<ContactInfoResultDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(ContactInfoCreateDto contactInfoAddDto);
        Task<IResult> UpdateAsync(ContactInfoUpdateDto contactInfoUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
