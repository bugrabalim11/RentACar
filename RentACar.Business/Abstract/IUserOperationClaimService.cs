using RentACar.Core.Utilities.Results;
using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.Business.Abstract
{
    public interface IUserOperationClaimService
    {
        Task<IDataResult<List<UserOperationClaimResultDto>>> GetAllAsync();
        Task<IDataResult<UserOperationClaimResultDto>> GetByIdAsync(int id);
        Task<IDataResult<List<UserOperationClaimDetailDto>>> GetMyOperationClaimsAsync(int userId);
        Task<IDataResult<List<UserOperationClaimDetailDto>>> GetClaimDetailsAsync();
        Task<IDataResult<UserOperationClaimUpdateDto>> GetUpdateDtoByUserIdAsync(int userId);
    }
}
