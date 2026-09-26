using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;
using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IUserOperationClaimService
    {
        Task<IDataResult<List<UserOperationClaimResultDto>>> GetAllAsync();
        Task<IDataResult<UserOperationClaimResultDto>> GetByIdAsync(int id);
        Task<IDataResult<List<UserOperationClaimDetailDto>>> GetMyOperationClaimsAsync(int userId);
        Task<IResult> AddAsync(UserOperationClaimCreateDto userOperationClaimAddDto);
        Task<IResult> UpdateAsync(UserOperationClaimUpdateDto userOperationClaimUpdateDto);
        Task<IResult> DeleteAsync(int id);

        // DİKKAT: Dışarıya çıplak liste değil, resmi kutumuz olan IDataResult içinde yolluyoruz!
        Task<IDataResult<List<UserOperationClaimDetailDto>>> GetClaimDetailsAsync();

        /// <summary>
        /// Verilen kullanıcı ID'sine ait yetki atama kaydını bulur ve güncelleme işlemlerinde (UpdateForAdmin) 
        /// kullanılmak üzere UserOperationClaimUpdateDto formatında geri döner.
        /// </summary>
        Task<IDataResult<UserOperationClaimUpdateDto>> GetUpdateDtoByUserIdAsync(int userId);
    }
}
