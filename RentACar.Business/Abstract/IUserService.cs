using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Results;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<UserListDto>>> GetAllAsync();
        Task<IDataResult<UserListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(UserAddDto userAddDto);

        // Güvenlik Şefinin (AuthManager) doğrudan kullanacağı, çıplak Entity kabul eden masa
        Task<IResult> AddAsync(User user);

        Task<IResult> UpdateAsync(UserUpdateDto userUpdateDto);
        Task<IResult> DeleteAsync(int id);

        Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user);

        // Customer için kullanıcı kayıtlı mı metodu
        Task<IResult> CheckIfUserExistsAsync(int id);
        Task<IResult> CheckIfEmailExistsAsync(string email);


        Task<IDataResult<User>> GetByMailAsync(string email);
    }
}
