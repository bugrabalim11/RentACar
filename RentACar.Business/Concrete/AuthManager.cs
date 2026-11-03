using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Aspects.Autofac.Transaction;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Utilities.Security.Hashing;
using RentACar.Core.Utilities.Security.Jwt;
using RentACar.Dtos.AuthDtos;
using RentACar.Dtos.UserDtos;
using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserAuthService _userAuthService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;
        private readonly IAdminUserOperationClaimService _adminUserOperationClaimService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly AuthBusinessRules _authBusinessRules;

        public AuthManager(IUserAuthService userAuthService, ITokenHelper tokenHelper, IMapper mapper, IAdminUserOperationClaimService adminUserOperationClaimService, IOperationClaimService operationClaimService, AuthBusinessRules authBusinessRules)
        {
            _userAuthService = userAuthService;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
            _adminUserOperationClaimService = adminUserOperationClaimService;
            _operationClaimService = operationClaimService;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<IResult> ChangePassword(int userId, UserChangePasswordDto userChangePasswordDto)
        {
            var user = await _userAuthService.GetByIdForAuthAsync(userId);
            if (!user.Success || user.Data == null)
            {
                throw new BusinessException("Kullanıcı bulunamadı.");
            }

            if (!HashingHelper.VerifyPasswordHash(userChangePasswordDto.OldPassword, user.Data.PasswordHash, user.Data.PasswordSalt))
            {
                throw new BusinessException("Eski şifreniz hatalı!");
            }

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userChangePasswordDto.NewPassword, out passwordHash, out passwordSalt);
            user.Data.PasswordHash = passwordHash;
            user.Data.PasswordSalt = passwordSalt;

            var result = await _userAuthService.UpdateForAuthAsync(user.Data);
            return new SuccessResult(result.Message ?? "Şifre başarıyla değiştirildi.");
        }

        public async Task<IDataResult<AccessToken>> CreateAccessToken(User user)
        {
            var claimsResult = await _userAuthService.GetClaimsAsync(user);
            var accessToken = _tokenHelper.CreateToken(user, claimsResult.Data);
            return new SuccessDataResult<AccessToken>(accessToken, "Erişim bileti (Token) başarıyla oluşturuldu.");
        }

        public async Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = await _userAuthService.GetByMailAsync(userForLoginDto.Email);
            if (!userToCheck.Success || userToCheck.Data == null)
            {
                throw new BusinessException("E-posta veya şifre hatalı!");
            }

            IResult? result = BusinessRules.Run(
                _authBusinessRules.CheckIfUserActive(userToCheck.Data.IsDeleted)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "Kullanıcı pasif durumda.");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                throw new BusinessException("E-posta veya şifre hatalı!");
            }
            return new SuccessDataResult<User>(userToCheck.Data, "Sisteme başarıyla giriş yapıldı.");
        }

        [TransactionScopeAspect]
        public async Task<IDataResult<User>> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            userForRegisterDto.Email = userForRegisterDto.Email.Trim().ToLower();
            IResult? result = BusinessRules.Run(
                await _authBusinessRules.CheckIfEmailExistsAsync(userForRegisterDto.Email)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "Bu kullancı kayıtlı! Lütfen başka deneyiniz.");
            }

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = _mapper.Map<User>(userForRegisterDto);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.IsDeleted = false;  

            await _userAuthService.AddAsync(user);

            var defaultRole = await _operationClaimService.GetIdByNameAsync("user");
            UserOperationClaimCreateDto userOperationClaimCreateDto = new UserOperationClaimCreateDto
            {
                UserId = user.Id,
                OperationClaimId = defaultRole.Data
            };
            await _adminUserOperationClaimService.AddAsync(userOperationClaimCreateDto);

            return new SuccessDataResult<User>(user, "Kayıt işlemi başarıyla tamamlandı.");
        }
    }
}
