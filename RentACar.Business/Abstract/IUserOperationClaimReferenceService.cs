using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IUserOperationClaimReferenceService
    {
        Task<IResult> CheckIfOperationClaimInUseAsync(int operationClaimId);
    }
}
