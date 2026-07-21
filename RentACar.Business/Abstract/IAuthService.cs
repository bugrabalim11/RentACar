using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Utilities.Security.Jwt;

namespace RentACar.Business.Abstract
{
    public interface IAuthService
    {
        // Adamdan detaylı formu (UserForRegisterDto) alır,
        // arka planda şifresini püre yapar (Hash) ve veritabanına yeni bir User olarak ekler.
        Task<IDataResult<User>> Register(UserForRegisterDto userForRegisterDto, string password);


        // Adamdan kısa formu (UserForLoginDto) alır,
        // şifresini püre yapıp veritabanındakiyle karşılaştırır. Doğruysa adamı (User) içeri alır.
        Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto);


        // Adam kayıt olmadan önce kapıdan bağırır:
        // "Bu e-posta içeride var mı?". Fedai listeye bakar, varsa "Kardeşim bu mail zaten kayıtlı" der.
        Task<IResult> UserExist(string email);


        // Adam başarıyla giriş yaptıktan veya kayıt olduktan sonra,
        // matbaayı (JwtHelper) çalıştırıp adamın eline o parlak AccessToken'ı (VIP bileti) verir.
        Task<IDataResult<AccessToken>> CreateAccessToken(User user);
    }
}
