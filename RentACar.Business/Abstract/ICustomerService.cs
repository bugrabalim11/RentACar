using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CustomerDtos;

namespace RentACar.Business.Abstract
{
    public interface ICustomerService
    {
        Task<IDataResult<List<CustomerResultDto>>> GetAllAsync();
        Task<IDataResult<CustomerDetailDto>> GetByIdAsync(int id);
        Task<IDataResult<CustomerDetailDto>> GetMyCustomerProfileAsync(int userId);
        Task<IDataResult<int>> AddAsync(int userId, CustomerCreateDto customerAddDto);
        Task<IDataResult<int>> AddForAdminAsync(CustomerCreateByAdminDto customerAddByAdminDto);
        Task<IResult> UpdateAsync(CustomerUpdateByAdminDto customerUpdateDto);
        Task<IResult> UpdateMyProfileAsync(int userId, CustomerUpdateMyProfileDto customerUpdateMyProfileDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> CheckIfCustomerExistsByIdAsync(int customerId);
    }
}
