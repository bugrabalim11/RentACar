using RentACar.Core.Utilities.Results;
using RentACar.Dtos.ContactMessageDtos;
using RentACar.Dtos.OfficeDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IContactMessageService
    {
        Task<IDataResult<List<ContactMessageListDto>>> GetAllAsync();
        Task<IDataResult<ContactMessageListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(ContactMessageAddDto contactMessageAddDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> ChangeIsReadStatusAsync(int id);
    }
}
