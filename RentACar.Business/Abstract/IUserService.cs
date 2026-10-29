using RentACar.Core.Utilities.Results;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<UserResultDto>>> GetAllAsync();
        Task<IDataResult<UserResultDto>> GetByIdAsync(int id);
        Task<IDataResult<UserResultDto>> GetMyProfile(int id);
        Task<IResult> UpdateMyProfileAsync(int userId, UserProfileUpdateDto userProfileUpdateDto);
        Task<IResult> DeleteMyAccountAsync(int id);
        Task<IResult> CheckIfUserExistsAsync(int userId);
    }
}
