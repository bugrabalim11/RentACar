using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IReferenceCheckService
    {
        Task<IResult> CheckIfUserExistsAsync(int userId);
        Task<IResult> CheckIfOfficeHasRentalsAsync(int officeId);
        Task<IResult> CheckIfOperationClaimExistsAsync(int operationClaimId);
    }
}
