using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IOperationClaimService _operationClaimService;

        public UserOperationClaimManager(IUserOperationClaimRepository userOperationClaimRepository, IMapper mapper, IUserService userService, IOperationClaimService operationClaimService)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _mapper = mapper;
            _userService = userService;
            _operationClaimService = operationClaimService;
        }

        public async Task<IResult> AddAsync(UserOperationClaimAddDto userOperationClaimAddDto)
        {
            // İŞ KURALI(BUSINESS RULE) KONTROLÜ - YENİ EKLENEN KISIM
            IResult? result = BusinessRules.Run(
            await CheckIfUserHasThisClaimAlreadyAsync(userOperationClaimAddDto.UserId, userOperationClaimAddDto.OperationClaimId),
            await CheckIfOperationClaimExistsAsync(userOperationClaimAddDto.OperationClaimId),
            await CheckIfUserExistsAsync(userOperationClaimAddDto.UserId)
            );
            if (result != null)
            {
                return result;
            }

            var userOperationClaim = _mapper.Map<UserOperationClaim>(userOperationClaimAddDto);
            await _userOperationClaimRepository.AddAsync(userOperationClaim);
            return new SuccessResult("Kullanıcıya yetki başarıyla atandı.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingUserOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.Id == id);
            if (existingUserOperationClaim == null)
            {
                return new ErrorResult("Silinmek istenen yetki ataması bulunamadı.");
            }

            existingUserOperationClaim.IsDeleted = true;
            existingUserOperationClaim.DeletedDate = DateTime.UtcNow;
            await _userOperationClaimRepository.UpdateAsync(existingUserOperationClaim);
            return new SuccessResult("Kullanıcının yetkisi başarıyla kaldırıldı.");
        }

        public async Task<IDataResult<List<UserOperationClaimListDto>>> GetAllAsync()
        {
            var userOperationClaims = await _userOperationClaimRepository.GetAllAsync();
            var userOperationClaimDtos = _mapper.Map<List<UserOperationClaimListDto>>(userOperationClaims);
            return new SuccessDataResult<List<UserOperationClaimListDto>>(userOperationClaimDtos, "Tüm kullanıcı yetkileri listelendi.");
        }

        public async Task<IDataResult<UserOperationClaimListDto>> GetByIdAsync(int id)
        {
            var userOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.Id == id);
            if (userOperationClaim == null)
            {
                return new ErrorDataResult<UserOperationClaimListDto>("Belirtilen yetki ataması bulunamadı.");
            }

            var userOperationClaimDto = _mapper.Map<UserOperationClaimListDto>(userOperationClaim);
            return new SuccessDataResult<UserOperationClaimListDto>(userOperationClaimDto, "Yetki ataması başarıyla getirildi.");
        }

        public async Task<IDataResult<List<UserOperationClaimDetailDto>>> GetClaimDetailsAsync()
        {
            // 1. Telsizle depocuya (Repository) seslen ve özel tabağı (JOIN sorgusunu) iste
            var claimDetails = await _userOperationClaimRepository.GetClaimDetailsAsync();

            // 2. Gelen bu özel tabağı, şirketimizin resmi "Başarılı Sonuç" kutusuna koy ve mesajını ekle
            return new SuccessDataResult<List<UserOperationClaimDetailDto>>(claimDetails, "Kullanıcı yetkileri detaylı olarak listelendi.");
        }

        public async Task<IResult> UpdateAsync(UserOperationClaimUpdateDto userOperationClaimUpdateDto)
        {
            IResult? result = BusinessRules.Run(
            await CheckIfUserHasThisClaimAlreadyForUpdateAsync(userOperationClaimUpdateDto.UserId, userOperationClaimUpdateDto.OperationClaimId, userOperationClaimUpdateDto.Id),
            await CheckIfOperationClaimExistsAsync(userOperationClaimUpdateDto.OperationClaimId),
            await CheckIfUserExistsAsync(userOperationClaimUpdateDto.UserId)
            );
            if (result != null)
            {
                return result;
            }

            var existingUserOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.Id == userOperationClaimUpdateDto.Id);
            if (existingUserOperationClaim == null)
            {
                return new ErrorResult("Güncellenmek istenen yetki ataması bulunamadı.");
            }

            _mapper.Map(userOperationClaimUpdateDto, existingUserOperationClaim);
            await _userOperationClaimRepository.UpdateAsync(existingUserOperationClaim);
            return new SuccessResult("Kullanıcı yetkisi başarıyla güncellendi.");
        }

        private async Task<IResult> CheckIfUserHasThisClaimAlreadyAsync(int userId, int operationClaimId)
        {
            // Telsizle depoya sor: "Böyle bir kayıt var mı? (Evet/Hayır)"
            // AnyAsync sana true ya da false dönecek.
            var result = await _userOperationClaimRepository.AnyAsync(uoc => uoc.UserId == userId && uoc.OperationClaimId == operationClaimId);

            // result zaten true veya false olduğu için direkt if (result) yazabiliriz.
            // "Eğer result true ise (yani kayıt VARSA)" demek ki adam zaten bu rütbeye sahip!
            if (result)
            {
                return new ErrorResult("Kullanıcı zaten bu yetkiye sahip.");
            }
            return new SuccessResult();
        }



        // Bu adama ait ve bu role sahip, AMA Id'si benim güncellediğim kaydın Id'sinden farklı (!=) başka bir kayıt var mı?
        private async Task<IResult> CheckIfUserHasThisClaimAlreadyForUpdateAsync(int userId, int operationClaimId, int currentId)
        {
            bool isExist = await _userOperationClaimRepository.AnyAsync(x => x.UserId == userId && x.OperationClaimId == operationClaimId && x.Id != currentId);
            if (isExist)
            {
                return new ErrorResult("Bu yetki zaten kullanıcıda mevcut!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfUserExistsAsync(int userId)
        {
            var existingUser = await _userService.GetByIdAsync(userId);
            if (!existingUser.Success)
            {
                return new ErrorResult(existingUser.Message ?? "Bu kullanıcı bulunamadı! Lüten tekrar deneyiniz.");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfOperationClaimExistsAsync(int operationClaimId)
        {
            var existingOperationClaim = await _operationClaimService.GetByIdAsync(operationClaimId);
            if (!existingOperationClaim.Success)
            {
                return new ErrorResult(existingOperationClaim.Message ?? "Bu statü bulunamadı! Lüten tekrar deneyiniz.");
            }
            return new SuccessResult();
        }
    }
}
