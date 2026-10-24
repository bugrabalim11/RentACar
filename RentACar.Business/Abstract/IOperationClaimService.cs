using RentACar.Core.Utilities.Results;
using RentACar.Dtos.OperationClaimDtos;

namespace RentACar.Business.Abstract
{
    public interface IOperationClaimService
    {
        Task<IDataResult<List<OperationClaimResultDto>>> GetAllAsync();
        Task<IDataResult<OperationClaimResultDto>> GetByIdAsync(int id);
        Task<IDataResult<int>> AddAsync(OperationClaimCreateDto operationClaimAddDto);
        Task<IResult> UpdateAsync(OperationClaimUpdateDto operationClaimUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
