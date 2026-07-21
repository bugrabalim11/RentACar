using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Utilities.Security.Hashing;
using RentACar.Core.Utilities.Security.Jwt;
using RentACar.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

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

        public Task<IDataResult<AccessToken>> CreateAccessToken(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {
            throw new NotImplementedException();
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

        public Task<IResult> UserExist(string email)
        {
            throw new NotImplementedException();
        }
    }
}
