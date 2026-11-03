using RentACar.Core.Exceptions;
using System.Security.Claims;

namespace RentACar.Core.Extensions
{
    /// <summary>
    /// Kimlik doğrulama süreçlerinde kullanılan ClaimsPrincipal nesnesi için genişletme (Extension) metotlarını içerir.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userIdString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new BusinessException("Kimlik doğrulama hatası! Geçerli bir token bulunamadı!");
            }
            return Convert.ToInt32(userIdString);
        }
    }
}
