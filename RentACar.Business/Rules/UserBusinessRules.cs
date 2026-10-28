using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class UserBusinessRules
    {
        private readonly IUserRepository _userRepository;

        public UserBusinessRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IResult> CheckIfEmailExistsForUpdateAsync(string email, int currentUserId)
        {
            bool isExist = await _userRepository.AnyAsync(x => x.Email == email && x.Id != currentUserId, ignoreQueryFilters: true);
            if (isExist)
            {
                return new ErrorResult("Bu e-posta adresi zaten kayıtlı! Lütfen başka deneyiniz.");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfEmailExistsAsync(string email)
        {
            bool existingEmail = await _userRepository.AnyAsync(x => x.Email == email, ignoreQueryFilters: true);
            if (existingEmail)
            {
                return new ErrorResult("Bu e-posta adresi zaten kayıtlı! Lütfen başka deneyiniz.");
            }
            return new SuccessResult();
        }
    }
}