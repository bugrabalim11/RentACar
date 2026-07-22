using RentACar.Core.Entities.DTOs.OperationClaimDtos;
using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IOperationClaimService
    {
        Task<IDataResult<List<OperationClaimListDto>>> GetAllAsync();
        Task<IDataResult<OperationClaimListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(OperationClaimAddDto operationClaimAddDto);
        Task<IResult> UpdateAsync(OperationClaimUpdateDto operationClaimUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
