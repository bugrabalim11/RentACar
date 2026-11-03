using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Utilities.Security.Jwt;
using RentACar.Dtos.AuthDtos;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<User>> Register(UserForRegisterDto userForRegisterDto, string password);
        Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto);
        Task<IDataResult<AccessToken>> CreateAccessToken(User user);
        Task<IResult> ChangePassword(int userId, UserChangePasswordDto userChangePasswordDto);
    }
}
