using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class OperationClaimBusinessRules
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IUserOperationClaimReferenceService _userOperationClaimReferenceService;

        public OperationClaimBusinessRules(IOperationClaimRepository operationClaimRepository, IUserOperationClaimReferenceService userOperationClaimReferenceService)
        {
            _operationClaimRepository = operationClaimRepository;
            _userOperationClaimReferenceService = userOperationClaimReferenceService;
        }

        public async Task<IResult> CheckIfOperationClaimExistsAsync(string operationClaim)
        {
            bool existOperationClaim = await _operationClaimRepository.AnyAsync(x => x.Name.ToLower() == operationClaim.ToLower());
            if (existOperationClaim)
            {
                return new ErrorResult("Bu statü sistemde kayıtlı! Lütfen başka statü girmeyi deneyin.");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfOperationClaimExistsForUpdateAsync(string operationClaim, int operationClaimId)
        {
            bool isExist = await _operationClaimRepository.AnyAsync(x => x.Name.ToLower() == operationClaim.ToLower() && x.Id != operationClaimId);
            if (isExist)
            {
                return new ErrorResult("Bu statü sistemde kayıtlı! Lütfen başka statü girmeyi deneyin.");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfOperationClaimInUseAsync(int operationClaimId)
        {
            return await _userOperationClaimReferenceService.CheckIfOperationClaimInUseAsync(operationClaimId);
        }

        public IResult CheckIfOperationClaimNameIsAdmin(string name)
        {
            // StringComparison.OrdinalIgnoreCase: "Büyük/küçük harfe takılma ve işletim sisteminin diline
            // (Türkçe/İngilizce vb.) bakmadan evrensel karşılaştır" demektir. Hafızada yeni kutu açmaz, %100 performanslıdır!
            if (name.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return new ErrorResult("Sistemin temel yetkileri üzerinde değişiklik yapılmasına izin verilmez!");
            }
            return new SuccessResult();
        }
    }
}
