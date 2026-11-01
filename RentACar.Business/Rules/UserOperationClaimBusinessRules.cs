using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class UserOperationClaimBusinessRules
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserService _userService;

        public UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository, IOperationClaimService operationClaimService, IUserService userService)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _operationClaimService = operationClaimService;
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

        public async Task<IResult> CheckIfOperationClaimExistsByIdAsync(int operationClaimId)
        {
            return await _operationClaimService.CheckIfOperationClaimExistsByIdAsync(operationClaimId);
        }

        public async Task<IResult> CheckIfUserExistsAsync(int userId)
        {
            return await _userService.CheckIfUserExistsAsync(userId);
        }
    }
}
