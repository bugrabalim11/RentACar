using System.IdentityModel.Tokens.Jwt;

namespace RentACar.MVC.Extensions
{
    public static class JwtTokenExtensions
    {
        /// <summary>
        /// API' dan gelen şifreli JWT token' ı çözer ve içindeki Rol bilgisini metin olarak döndürür.
        /// </summary>
        /// <param name="token">Çözülecek olan şifreli token metni</param>
        /// <returns>Kullanıcının rolü (örn: "admin" veya "user"). Rol bulunamazsa null döner.</returns>
        public static string? GetRoleFromJwt(this string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (string.IsNullOrEmpty(token)) return null;

            var jwtToken = handler.ReadJwtToken(token);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            return roleClaim?.Value;
        }
    }
}
