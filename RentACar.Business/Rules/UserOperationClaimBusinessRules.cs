using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class UserOperationClaimBusinessRules
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IUserRepository _userRepository;

        public UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository, IOperationClaimRepository operationClaimRepository, IUserRepository userRepository)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _operationClaimRepository = operationClaimRepository;
            _userRepository = userRepository;
        }

        public async Task<IResult> CheckIfUserHasThisClaimAlreadyAsync(int userId, int operationClaimId)
        {
            var result = await _userOperationClaimRepository.AnyAsync(uoc => uoc.UserId == userId && uoc.OperationClaimId == operationClaimId);
            if (result)
            {
                return new ErrorResult("Kullanıcı zaten bu yetkiye sahip.");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfUserHasThisClaimAlreadyForUpdateAsync(int userId, int operationClaimId, int currentId)
        {
            bool isExist = await _userOperationClaimRepository.AnyAsync(x => x.UserId == userId && x.OperationClaimId == operationClaimId && x.Id != currentId);
            if (isExist)
            {
                return new ErrorResult("Bu yetki zaten kullanıcıda mevcut!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfOperationClaimExistsAsync(int operationClaimId)
        {
            bool existOperationClaim = await _operationClaimRepository.AnyAsync(x => x.Id == operationClaimId);
            if (!existOperationClaim)
            {
                return new ErrorResult("Böyle bir yetki sistemde bulunamadı!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfUserExistsAsync(int userId)
        {
            bool existingUser = await _userRepository.AnyAsync(x => x.Id == userId);
            if (existingUser)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu kullanıcı sistemde bulunamadı!");
        }
    }
}
