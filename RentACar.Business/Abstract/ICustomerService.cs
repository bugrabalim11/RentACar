using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CustomerDtos;

namespace RentACar.Business.Abstract
{
    public interface ICustomerService
    {
        Task<IDataResult<CustomerDetailDto>> GetMyCustomerProfileAsync(int userId);
        Task<IDataResult<int>> AddAsync(int userId, CustomerCreateDto customerAddDto);
        Task<IResult> UpdateMyProfileAsync(int userId, CustomerUpdateMyProfileDto customerUpdateMyProfileDto);
        Task<IResult> CheckIfCustomerExistsByIdAsync(int customerId);
    }
}
