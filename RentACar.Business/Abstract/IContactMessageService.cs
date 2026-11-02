using RentACar.Core.Utilities.Results;
using RentACar.Dtos.ContactMessageDtos;

namespace RentACar.Business.Abstract
{
    public interface IContactMessageService
    {
        Task<IDataResult<int>> AddAsync(ContactMessageCreateDto contactMessageAddDto);
    }
}
