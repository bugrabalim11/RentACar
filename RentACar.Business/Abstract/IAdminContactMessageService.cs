using RentACar.Core.Utilities.Results;
using RentACar.Dtos.ContactMessageDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminContactMessageService
    {
        Task<IDataResult<List<ContactMessageResultDto>>> GetAllAsync();
        Task<IDataResult<ContactMessageResultDto>> GetByIdAsync(int id);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> MarkAsReadAsync(int id);
    }
}
