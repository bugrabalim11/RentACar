using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;
using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IUserOperationClaimService
    {
        Task<IDataResult<List<UserOperationClaimListDto>>> GetAllAsync();
        Task<IDataResult<UserOperationClaimListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(UserOperationClaimAddDto userOperationClaimAddDto);
        Task<IResult> UpdateAsync(UserOperationClaimUpdateDto userOperationClaimUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
