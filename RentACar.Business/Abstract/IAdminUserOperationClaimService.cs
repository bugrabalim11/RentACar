using RentACar.Core.Utilities.Results;
using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminUserOperationClaimService
    {
        Task<IDataResult<int>> AddAsync(UserOperationClaimCreateDto userOperationClaimAddDto);
        Task<IResult> UpdateAsync(UserOperationClaimUpdateDto userOperationClaimUpdateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> SetUserClaimAsync(int userId, int? operationClaimId);
    }
}
