using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CustomerDtos;

namespace RentACar.Business.Abstract
{
    public interface ICustomerService
    {
        Task<IDataResult<List<CustomerListDto>>> GetAllAsync();
        Task<IDataResult<CustomerDetailDto>> GetByIdAsync(int id);
        Task<IDataResult<CustomerDetailDto>> GetMyCustomerProfileAsync(int userId);
        Task<IResult> AddAsync(int userId, CustomerAddDto customerAddDto);
        Task<IResult> AddForAdminAsync(CustomerAddByAdminDto customerAddByAdminDto);
        Task<IResult> UpdateAsync(CustomerUpdateDto customerUpdateDto);
        Task<IResult> UpdateMyProfileAsync(int userId, CustomerUpdateMyProfileDto customerUpdateMyProfileDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> CheckIfCustomerExistsByIdAsync(int customerId);
    }
}
