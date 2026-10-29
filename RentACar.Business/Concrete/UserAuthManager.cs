using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class UserAuthManager : IUserAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserBusinessRules _userBusinessRules;

        public UserAuthManager(IUserRepository userRepository, UserBusinessRules userBusinessRules)
        {
            _userRepository = userRepository;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(User user)
        {
            await _userRepository.AddAsync(user);
            return new SuccessDataResult<int>(user.Id, "Kullancı güvenli bir şekilde sisteme eklendi.");
        }

        public async Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user)
        {
            var claims = await _userRepository.GetClaimsAsync(user);
            return new SuccessDataResult<List<OperationClaim>>(claims, "Kullancı yetkileri başarıyla getirildi.");
        }

        public async Task<IDataResult<User>> GetByMailAsync(string email)
        {
            var user = await _userRepository.GetAsync(u => u.Email == email, ignoreQueryFilters: true);
            if (user == null)
            {
                return new ErrorDataResult<User>("Bu e-posta adresine sahip kullanıcı bulunamadı.");
            }
            return new SuccessDataResult<User>(user);
        }

        public async Task<IDataResult<User>> GetByIdForAuthAsync(int id)
        {
            var user = await _userRepository.GetAsync(u => u.Id == id);
            if (user == null)
            {
                return new ErrorDataResult<User>("Bu ID'ye sahip kullanıcı bulunamadı.");
            }
            return new SuccessDataResult<User>(user);
        }

        public async Task<IResult> UpdateForAuthAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
            return new SuccessResult("Şifre başarıyla güncellendi.");
        }

        public async Task<IResult> CheckIfEmailExistsAsync(string email)
        {
            return await _userBusinessRules.CheckIfEmailExistsAsync(email);
        }
    }
}
