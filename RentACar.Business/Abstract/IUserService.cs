using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Entities.DTOs.UserDtos;

namespace RentACar.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<UserResultDto>>> GetAllAsync();
        Task<IDataResult<List<UserResultByAdminDto>>> GetAllForAdminAsync();
        Task<IDataResult<UserResultDto>> GetByIdAsync(int id);
        Task<IDataResult<UserResultDto>> GetMyProfile(int id);

        // Güvenlik Şefinin (AuthManager) doğrudan kullanacağı, çıplak Entity kabul eden masa
        Task<IDataResult<int>> AddAsync(User user);

        Task<IDataResult<UserUpdateByAdminDto>> GetByIdForUpdateAsync(int id);
        Task<IResult> UpdateForAdminAsync(UserUpdateByAdminDto userUpdateForAdminDto);
        Task<IDataResult<int>> CreateForAdminAsync(UserCreateByAdminDto userCreateForAdminDto);
        Task<IResult> UpdateMyProfileAsync(int userId, UserProfileUpdateDto userProfileUpdateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> RestoreAsync(int id);

        Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user);

        Task<IResult> CheckIfEmailExistsAsync(string email);
        Task<IDataResult<User>> GetByMailAsync(string email);

        Task<IDataResult<User>> GetByIdForAuthAsync(int id);
        Task<IResult> UpdateForAuthAsync(User user);
    }
}
