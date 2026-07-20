using RentACar.Core.Utilities.Results;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<UserListDto>>> GetAllAsync();
        Task<IDataResult<UserListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(UserAddDto userAddDto);
        Task<IResult> UpdateAsync(UserUpdateDto userUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
