using RentACar.Core.Utilities.Results;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminUserService
    {
        Task<IResult> UpdateForAdminAsync(UserUpdateByAdminDto userUpdateForAdminDto);
        Task<IDataResult<int>> CreateForAdminAsync(UserCreateByAdminDto userCreateForAdminDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> RestoreAsync(int id);
        Task<IDataResult<List<UserResultByAdminDto>>> GetAllForAdminAsync();
        Task<IDataResult<UserUpdateByAdminDto>> GetByIdForUpdateAsync(int id);
    }
}
