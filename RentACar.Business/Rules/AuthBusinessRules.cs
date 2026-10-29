using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Rules
{
    public class AuthBusinessRules
    {
        private readonly IUserAuthService _userAuthService;

        public AuthBusinessRules(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        public async Task<IResult> CheckIfEmailExistsAsync(string email)
        {
            return await _userAuthService.CheckIfEmailExistsAsync(email);
        }

        public IResult CheckIfUserActive(bool isDeleted)
        {
            if (isDeleted)
            {
                return new ErrorResult("Kullanıcı hesabınız pasif durumdadır, giriş yapamazsınız");
            }
            return new SuccessResult();
        }
    }
}
