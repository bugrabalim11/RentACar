using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.AuthDtos;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Utilities.Security.Hashing;
using RentACar.Core.Utilities.Security.Jwt;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IMapper mapper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
        }

        public async Task<IResult> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            var user = await _userService.GetByIdForAuthAsync(userChangePasswordDto.UserId);
            if (!user.Success || user.Data == null)
            {
                return new ErrorResult("Kullanıcı bulunamadı.");
            }

            if (!HashingHelper.VerifyPasswordHash(userChangePasswordDto.OldPassword, user.Data.PasswordHash, user.Data.PasswordSalt))
            {
                return new ErrorResult("Eski şifreniz hatalı!");
            }

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userChangePasswordDto.NewPassword, out passwordHash, out passwordSalt);
            user.Data.PasswordHash = passwordHash;
            user.Data.PasswordSalt = passwordSalt;

            var result = await _userService.UpdateForAuthAsync(user.Data);
            return new SuccessResult(result.Message ?? "Şifre başarıyla değiştirildi.");
        }

        public async Task<IDataResult<AccessToken>> CreateAccessToken(User user)
        {
            // 1. Adamın rollerini (VIP listesini) getir
            var claimsResult = await _userService.GetClaimsAsync(user);

            // 2. Matbaayı çalıştır ve Token'ı üret
            // Dikkat: claimsResult.Data diyerek IDataResult içindeki asıl List<OperationClaim> listesini matbaaya veriyoruz.
            var accessToken = _tokenHelper.CreateToken(user, claimsResult.Data);

            return new SuccessDataResult<AccessToken>(accessToken, "Erişim bileti (Token) başarıyla oluşturuldu.");
        }

        public async Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {
            // 2. Telsizle e-posta kontrolü
            var userToCheck = await _userService.GetByMailAsync(userForLoginDto.Email);
            if (!userToCheck.Success || userToCheck.Data == null)
            {
                // Senior Güvenlik Notu: Normalde hackerlar e-posta taraması yapmasın diye 
                // "E-posta veya şifre hatalı" diye genel bir mesaj döneriz. Ama şimdilik öğrenme aşamasındayız.
                return new ErrorDataResult<User>("Kullanıcı bulunamadı.");
            }

            IResult? result = BusinessRules.Run(CheckIfUserActive(userToCheck.Data.IsDeleted));
            if (result != null)
            {
                return new ErrorDataResult<User>(result.Message ?? "Kullanıcı pasif durumda.");
            }

            // 3. Şifre Doğrulama (Blender makinemizi tersine çalıştırıyoruz)
            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<User>("Parola hatası.");
            }

            return new SuccessDataResult<User>(userToCheck.Data, "Sisteme başarıyla giriş yapıldı.");
        }

        public async Task<IDataResult<User>> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            IResult? result = BusinessRules.Run(await _userService.CheckIfEmailExistsAsync(userForRegisterDto.Email));
            if (result != null)
            {
                // Senior Vizyonu: Sana söz verdiğim kutuyu veriyorum (ErrorDataResult),
                // İçine veri (User) koyamıyorum ama asistanın (result) getirdiği hata mesajını kutunun üstüne yazıyorum!
                return new ErrorDataResult<User>(result.Message ?? "Bu kullancı kayıtlı! Lütfen başka deneyiniz.");
            }

            // 1. Blender Makinesi: Şifreyi püre yap (out ile kavanozları dolduruyoruz)
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // 2. Çevirmen: Formu gerçek bir varlığa dönüştür
            var user = _mapper.Map<User>(userForRegisterDto);

            // 3. Mühürleme: Güvenlik bilgilerini manuel olarak nesneye zerk et
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.IsDeleted = false;  // Sisteme ilk kayıt olanı aktif yapıyoruz


            await _userService.AddAsync(user);
            return new SuccessDataResult<User>(user, "Kayıt işlemi başarıyla tamamlandı.");
        }

        private IResult CheckIfUserActive(bool isDeleted)
        {
            if (isDeleted)
            {
                return new ErrorResult("Kullanıcı hesabınız pasif durumdadır, giriş yapamazsınız");
            }
            return new SuccessResult();
        }
    }
}
