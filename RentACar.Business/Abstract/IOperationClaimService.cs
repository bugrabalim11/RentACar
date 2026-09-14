using RentACar.Core.Entities.DTOs.OperationClaimDtos;
using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IOperationClaimService
    {
        Task<IDataResult<List<OperationClaimResultDto>>> GetAllAsync();
        Task<IDataResult<OperationClaimResultDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(OperationClaimCreateDto operationClaimAddDto);
        Task<IResult> UpdateAsync(OperationClaimUpdateDto operationClaimUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
