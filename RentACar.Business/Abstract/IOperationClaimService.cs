using RentACar.Core.Utilities.Results;
using RentACar.Dtos.OperationClaimDtos;

namespace RentACar.Business.Abstract
{
    public interface IOperationClaimService
    {
        Task<IDataResult<List<OperationClaimResultDto>>> GetAllAsync();
        Task<IDataResult<OperationClaimResultDto>> GetByIdAsync(int id);
    }
}
