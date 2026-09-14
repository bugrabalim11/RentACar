using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Entities.DTOs.UserDtos;

namespace RentACar.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<UserResultDto>>> GetAllAsync();
        Task<IDataResult<List<UserResultForAdminDto>>> GetAllForAdminAsync();
        Task<IDataResult<UserResultDto>> GetByIdAsync(int id);
        Task<IDataResult<UserResultDto>> GetMyProfile(int id);

        // Güvenlik Şefinin (AuthManager) doğrudan kullanacağı, çıplak Entity kabul eden masa
        Task<IResult> AddAsync(User user);

        Task<IResult> UpdateForAdminAsync(UserUpdateForAdminDto userUpdateForAdminDto);
        Task<IResult> CreateForAdminAsync(UserCreateForAdminDto userCreateForAdminDto);
        Task<IResult> UpdateMyProfileAsync(int userId, UserProfileUpdateDto userProfileUpdateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> RestoreAsync(int id);

        Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user);

        // Customer için kullanıcı kayıtlı mı metodu
        Task<IResult> CheckIfUserExistsAsync(int id);
        Task<IResult> CheckIfEmailExistsAsync(string email);
        Task<IDataResult<User>> GetByMailAsync(string email);

        Task<IDataResult<User>> GetByIdForAuthAsync(int id);
        Task<IResult> UpdateForAuthAsync(User user);
    }
}
