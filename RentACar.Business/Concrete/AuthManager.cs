using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.AuthDtos;
using RentACar.Core.Entities.DTOs.UserDtos;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Utilities.Security.Hashing;
using RentACar.Core.Utilities.Security.Jwt;

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

        public async Task<IResult> ChangePassword(int userId, UserChangePasswordDto userChangePasswordDto)
        {
            // 1. KİMLİK TESPİTİ: Adamı veritabanından bul.
            var user = await _userService.GetByIdForAuthAsync(userId);
            if (!user.Success || user.Data == null)
            {
                throw new BusinessException("Kullanıcı bulunamadı.");
            }

            // 2. GÜVENLİK DUVARI: Eski şifre doğru mu? (Blender kontrolü)
            if (!HashingHelper.VerifyPasswordHash(userChangePasswordDto.OldPassword, user.Data.PasswordHash, user.Data.PasswordSalt))
            {
                throw new BusinessException("Eski şifreniz hatalı!");
            }

            // 3. YENİ MÜHÜR: Yeni şifreyi püre yap ve kavanozları (Hash/Salt) güncelle.
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userChangePasswordDto.NewPassword, out passwordHash, out passwordSalt);
            user.Data.PasswordHash = passwordHash;
            user.Data.PasswordSalt = passwordSalt;

            // 4. KAYIT: Güncel bilgileri veritabanına işle.
            var result = await _userService.UpdateForAuthAsync(user.Data);
            return new SuccessResult(result.Message ?? "Şifre başarıyla değiştirildi.");
        }

        public async Task<IDataResult<AccessToken>> CreateAccessToken(User user)
        {
            // 1. YETKİ KONTROLÜ: Adamın rollerini (VIP listesini) getir.
            var claimsResult = await _userService.GetClaimsAsync(user);

            // 2. MATBAA: VIP listesini (claimsResult.Data) matbaaya ver ve Token'ı (Bileti) üret.
            var accessToken = _tokenHelper.CreateToken(user, claimsResult.Data);

            return new SuccessDataResult<AccessToken>(accessToken, "Erişim bileti (Token) başarıyla oluşturuldu.");
        }

        public async Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {
            // 1. E-POSTA KONTROLÜ: Kullanıcı var mı?
            // Hacker'ı kör etmek için bulunamama durumunda da "E-posta veya şifre hatalı" diyoruz.
            var userToCheck = await _userService.GetByMailAsync(userForLoginDto.Email);
            if (!userToCheck.Success || userToCheck.Data == null)
            {
                throw new BusinessException("E-posta veya şifre hatalı!");
            }

            // 2. AKTİFLİK KONTROLÜ: İş kuralları motoru (Asistan) raporu inceler.
            IResult? result = BusinessRules.Run(CheckIfUserActive(userToCheck.Data.IsDeleted));
            if (result != null)
            {
                // Asistan hata bulursa kırmızı alarma bas!
                throw new BusinessException(result.Message ?? "Kullanıcı pasif durumda.");
            }

            // 3. ŞİFRE KONTROLÜ: Blender makinemizi tersine çalıştırıyoruz.
            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                throw new BusinessException("E-posta veya şifre hatalı!");
            }

            // 4. ZAFER: Bütün güvenlik duvarları aşıldı, mutlu son.
            return new SuccessDataResult<User>(userToCheck.Data, "Sisteme başarıyla giriş yapıldı.");
        }

        public async Task<IDataResult<User>> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            // 1. E-POSTA TEMİZLİĞİ VE KONTROLÜ
            userForRegisterDto.Email = userForRegisterDto.Email.Trim().ToLower();

            IResult? result = BusinessRules.Run(await _userService.CheckIfEmailExistsAsync(userForRegisterDto.Email));
            if (result != null)
            {
                // Asistan (result) hata raporu getirirse kırmızı alarma bas (Middleware tetiklensin).
                throw new BusinessException(result.Message ?? "Bu kullancı kayıtlı! Lütfen başka deneyiniz.");
            }

            // 2. BLENDER MAKİNESİ: Şifreyi püre yap (out ile kavanozları dolduruyoruz).
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // 3. ÇEVİRMEN: DTO formunu gerçek bir veritabanı varlığına (User) dönüştür.
            var user = _mapper.Map<User>(userForRegisterDto);

            // 4. MÜHÜRLEME: Güvenlik bilgilerini manuel olarak nesneye zerk et.
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.IsDeleted = false;  // Sisteme ilk kayıt olanı aktif yapıyoruz

            // 5. KAYIT: Yeni kullanıcıyı veritabanına ekle.
            await _userService.AddAsync(user);
            return new SuccessDataResult<User>(user, "Kayıt işlemi başarıyla tamamlandı.");
        }

        // KURAL USTASI (Sadece rapor tutar, kırmızı alarma basmaz)
        // O yüzden ErrrorResult kullandık
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
