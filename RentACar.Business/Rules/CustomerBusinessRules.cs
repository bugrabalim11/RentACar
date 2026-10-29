using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class CustomerBusinessRules
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserService _userService;

        public CustomerBusinessRules(ICustomerRepository customerRepository, IUserService userService)
        {
            _customerRepository = customerRepository;
            _userService = userService;
        }

        public async Task<IResult> CheckIfNationalIdentityExistsForUpdate(string nationalId, int currentCustomerId)
        {
            bool existingNationalId = await _customerRepository.AnyAsync(x => x.NationalIdentity == nationalId && x.Id != currentCustomerId);
            if (existingNationalId)
            {
                return new ErrorResult("Bu kimlik numarası zaten kayıtlı!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfNationalIdentityExists(string nationalId)
        {
            bool existingNationalId = await _customerRepository.AnyAsync(x => x.NationalIdentity == nationalId);
            if (existingNationalId)
            {
                return new ErrorResult("Bu kimlik numarası zaten kayıtlı! Lütfen tekrar deneyiniz.");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfUserAlreadyHasCustomerProfileAsync(int userId)
        {
            bool customerIsExist = await _customerRepository.AnyAsync(x => x.UserId == userId);
            if (customerIsExist)
            {
                return new ErrorResult("Bu kullanıcıya ait bir müşteri profili sistemde zaten mevcut!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfCustomerExistsByIdAsync(int customerId)
        {
            bool customerIsExist = await _customerRepository.AnyAsync(x => x.Id == customerId);
            if (!customerIsExist)
            {
                return new ErrorResult("Sistemde böyle bir müşteri yok devam etmek için lütfen oluşturun!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfUserExistsAsync(int userId)
        {
            return await _userService.CheckIfUserExistsAsync(userId);
        }
    }
}
