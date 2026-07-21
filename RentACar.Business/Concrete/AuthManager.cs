using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs;
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
        private readonly IValidator<UserForRegisterDto> _registerValidator;
        private readonly IValidator<UserForLoginDto> _loginValidator;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IMapper mapper, IValidator<UserForRegisterDto> registerValidator, IValidator<UserForLoginDto> loginValidator)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
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
            var validationResult = await _loginValidator.ValidateAsync(userForLoginDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // 2. Telsizle e-posta kontrolü
            var userToCheck = await _userService.GetByMailAsync(userForLoginDto.Email);
            if (!userToCheck.Success || userToCheck.Data == null)
            {
                // Senior Güvenlik Notu: Normalde hackerlar e-posta taraması yapmasın diye 
                // "E-posta veya şifre hatalı" diye genel bir mesaj döneriz. Ama şimdilik öğrenme aşamasındayız.
                return new ErrorDataResult<User>("Kullanıcı bulunamadı.");
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
            var validationResult = await _registerValidator.ValidateAsync(userForRegisterDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // 1. Blender Makinesi: Şifreyi püre yap (out ile kavanozları dolduruyoruz)
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // 2. Çevirmen: Formu gerçek bir varlığa dönüştür
            var user = _mapper.Map<User>(userForRegisterDto);

            // 3. Mühürleme: Güvenlik bilgilerini manuel olarak nesneye zerk et
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Status = true;  // Sisteme ilk kayıt olanı aktif yapıyoruz

            // 5. Kayıt (Şimdilik altı kızarabilir, çünkü IUserService'te bu aşırı yükleme (overload) yok, birazdan ekleyeceğiz)
            await _userService.AddAsync(user);

            return new SuccessDataResult<User>(user, "Kayıt işlemi başarıyla tamamlandı.");
        }

        public async Task<IResult> UserExist(string email)
        {
            // 1. Telsizle içerideki müdüre soruyoruz: "Bu e-posta bizde var mı?"
            var userToCheck = await _userService.GetByMailAsync(email);

            // 2. Eğer müdür "Evet, böyle biri var" (Success) dönerse, adama HATA fırlatıyoruz!
            if (userToCheck.Success)
            {
                return new ErrorResult("Kullanıcı zaten mevcut.");
            }

            // 3. Adam sistemde yoksa, yol açık, kayıt işlemine devam edebilir.
            return new SuccessResult();
        }
    }
}
