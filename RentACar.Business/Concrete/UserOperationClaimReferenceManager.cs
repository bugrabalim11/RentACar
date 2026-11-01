using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class UserOperationClaimReferenceManager : IUserOperationClaimReferenceService
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;

        public UserOperationClaimReferenceManager(IUserOperationClaimRepository userOperationClaimRepository)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
        }

        public async Task<IResult> CheckIfOperationClaimInUseAsync(int operationClaimId)
        {
            bool isExist = await _userOperationClaimRepository.AnyAsync(x => x.OperationClaimId == operationClaimId);
            if (isExist)
            {
                return new ErrorResult("Bu rütbe şu an bazı kullanıcılar tarafından kullanılıyor. Silmek için önce kullanıcıların yetkilerini almalısınız!");
            }
            return new SuccessResult();
        }
    }
}
