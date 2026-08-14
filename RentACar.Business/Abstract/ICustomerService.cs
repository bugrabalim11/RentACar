using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CustomerDtos;

namespace RentACar.Business.Abstract
{
    public interface ICustomerService
    {
        Task<IDataResult<List<CustomerListDto>>> GetAllAsync();
        Task<IDataResult<CustomerDetailDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(CustomerAddDto customerAddDto);
        Task<IResult> UpdateAsync(CustomerUpdateDto customerUpdateDto);
        Task<IResult> UpdateMyProfileAsync(int userId, CustomerUpdateDto customerUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
