using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Rules
{
    public class AuthBusinessRule
    {
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
