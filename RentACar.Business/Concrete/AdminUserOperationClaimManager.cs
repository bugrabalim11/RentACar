using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.Business.Concrete
{
    public class AdminUserOperationClaimManager : IAdminUserOperationClaimService
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IMapper _mapper;
        private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;
        private readonly IUserOperationClaimService _userOperationClaimService;

        public AdminUserOperationClaimManager(IUserOperationClaimRepository userOperationClaimRepository, IMapper mapper,UserOperationClaimBusinessRules userOperationClaimBusinessRules,IUserOperationClaimService userOperationClaimService)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _mapper = mapper;
            _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            _userOperationClaimService = userOperationClaimService;
        }

        public async Task<IDataResult<int>> AddAsync(UserOperationClaimCreateDto userOperationClaimAddDto)
        {
            IResult? result = BusinessRules.Run(
            await _userOperationClaimBusinessRules.CheckIfUserHasThisClaimAlreadyAsync(userOperationClaimAddDto.UserId, userOperationClaimAddDto.OperationClaimId),
            await _userOperationClaimBusinessRules.CheckIfOperationClaimExistsAsync(userOperationClaimAddDto.OperationClaimId),
            await _userOperationClaimBusinessRules.CheckIfUserExistsAsync(userOperationClaimAddDto.UserId)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında bir hata oluştu!");
            }

            var userOperationClaim = _mapper.Map<UserOperationClaim>(userOperationClaimAddDto);
            await _userOperationClaimRepository.AddAsync(userOperationClaim);
            return new SuccessDataResult<int>(userOperationClaim.Id, "Kullanıcıya yetki başarıyla atandı.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingUserOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.Id == id);
            if (existingUserOperationClaim == null)
            {
                throw new BusinessException("Silinmek istenen yetki ataması bulunamadı.");
            }
            await _userOperationClaimRepository.DeleteAsync(existingUserOperationClaim);
            return new SuccessResult("Kullanıcının yetkisi başarıyla kaldırıldı.");
        }

        public async Task<IResult> SetUserClaimAsync(int userId, int? operationClaimId)
        {
            var claimResult = await _userOperationClaimService.GetUpdateDtoByUserIdAsync(userId);
            if (claimResult.Success)
            {
                // Durum A: Adamın zaten bir yetki kartı var.
                if (operationClaimId.HasValue)
                {
                    // Admin yeni bir rütbe seçmiş, kartı güncelliyoruz.
                    // .Value diyerek int? içindeki kesin int değerini alıyoruz (Hatanın çözümü!)
                    claimResult.Data.OperationClaimId = operationClaimId.Value;
                    await UpdateAsync(claimResult.Data);
                }
                else
                {
                    // Admin formda "Rütbesiz" seçeneğini seçmiş (null göndermiş). 
                    // Adamın mevcut rütbe kartını yırtıp atıyoruz!
                    await DeleteAsync(claimResult.Data.Id);
                }
            }
            else
            {
                if (operationClaimId.HasValue)
                {
                    // Durum B: Adamın önceden kartı YOKTU. Eğer Admin formdan yeni bir rütbe seçmişse, adama sıfırdan bir yetki kartı basıyoruz.
                    UserOperationClaimCreateDto userOperationClaimCreateDto = new UserOperationClaimCreateDto
                    {
                        UserId = userId,
                        OperationClaimId = operationClaimId.Value
                    };
                    await AddAsync(userOperationClaimCreateDto);
                }
            }
            return new SuccessResult("Kullanıcının yetkileri başarıyla ayarlandı.");
        }

        public async Task<IResult> UpdateAsync(UserOperationClaimUpdateDto userOperationClaimUpdateDto)
        {
            var existingUserOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.Id == userOperationClaimUpdateDto.Id);
            if (existingUserOperationClaim == null)
            {
                throw new BusinessException("Güncellenmek istenen yetki ataması bulunamadı.");
            }

            IResult? result = BusinessRules.Run(
                await _userOperationClaimBusinessRules.CheckIfUserHasThisClaimAlreadyForUpdateAsync(existingUserOperationClaim.UserId, userOperationClaimUpdateDto.OperationClaimId, userOperationClaimUpdateDto.Id),
                await _userOperationClaimBusinessRules.CheckIfOperationClaimExistsAsync(userOperationClaimUpdateDto.OperationClaimId),
                await _userOperationClaimBusinessRules.CheckIfUserExistsAsync(existingUserOperationClaim.UserId)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında bir hata oluştu!");
            }

            _mapper.Map(userOperationClaimUpdateDto, existingUserOperationClaim);
            await _userOperationClaimRepository.UpdateAsync(existingUserOperationClaim);
            return new SuccessResult("Kullanıcı yetkisi başarıyla güncellendi.");
        }
    }
}
