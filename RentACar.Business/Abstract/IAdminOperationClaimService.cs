using RentACar.Core.Utilities.Results;
using RentACar.Dtos.OperationClaimDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminOperationClaimService
    {
        Task<IDataResult<int>> AddAsync(OperationClaimCreateDto operationClaimAddDto);
        Task<IResult> UpdateAsync(OperationClaimUpdateDto operationClaimUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
