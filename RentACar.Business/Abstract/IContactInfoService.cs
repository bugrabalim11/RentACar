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
        Task<IDataResult<List<ContactInfoListDto>>> GetAllAsync();
        Task<IDataResult<ContactInfoListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(ContactInfoAddDto contactInfoAddDto);
        Task<IResult> UpdateAsync(ContactInfoUpdateDto contactInfoUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
