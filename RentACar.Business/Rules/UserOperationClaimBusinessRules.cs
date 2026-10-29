using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class UserOperationClaimBusinessRules
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IUserService _userService;

        public UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository, IOperationClaimRepository operationClaimRepository, IUserService userService)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _operationClaimRepository = operationClaimRepository;
            _userService = userService;
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
            return await _userService.CheckIfUserExistsAsync(userId);
        }
    }
}
