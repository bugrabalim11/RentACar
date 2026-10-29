using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CustomerDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminCustomerService
    {
        Task<IDataResult<List<CustomerResultDto>>> GetAllAsync();
        Task<IDataResult<CustomerDetailDto>> GetByIdAsync(int id);
        Task<IDataResult<int>> AddForAdminAsync(CustomerCreateByAdminDto customerAddByAdminDto);
        Task<IResult> UpdateAsync(CustomerUpdateByAdminDto customerUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
