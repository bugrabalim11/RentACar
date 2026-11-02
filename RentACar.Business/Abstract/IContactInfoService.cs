using RentACar.Core.Utilities.Results;
using RentACar.Dtos.ContactInfoDtos;

namespace RentACar.Business.Abstract
{
    public interface IContactInfoService
    {
        Task<IDataResult<List<ContactInfoResultDto>>> GetAllAsync();
        Task<IDataResult<ContactInfoResultDto>> GetByIdAsync(int id);
    }
}
