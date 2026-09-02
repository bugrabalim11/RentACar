using RentACar.Core.Utilities.Results;
using RentACar.Dtos.ContactMessageDtos;

namespace RentACar.Business.Abstract
{
    public interface IContactMessageService
    {
        Task<IDataResult<List<ContactMessageListDto>>> GetAllAsync();
        Task<IDataResult<ContactMessageListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(ContactMessageAddDto contactMessageAddDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> ChangeIsReadStatusAsync(int id);
        Task<IResult> MarkAsReadAsync(int id);
    }
}
