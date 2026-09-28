using RentACar.Core.Exceptions;
using System.Security.Claims;

namespace RentACar.Core.Extensions
{
    // SENİOR NOTU 1: Bir sınıfa "Extension (Genişletme)" diyebilmemiz için KESİNLİKLE 'public static' olması zorundadır!
    // Static demek, bu sınıfın bir "fabrika" gibi çalışması ve "new" kelimesiyle üretilmesine gerek kalmaması demektir.
    public static class ClaimsPrincipalExtensions
    {
        // SENİOR NOTU 2: 'this ClaimsPrincipal claimsPrincipal' kelimesi sihirli kancanın ta kendisidir.
        // Microsoft'a diyoruz ki: "Senin hazır ClaimsPrincipal (User) nesnene 'GetUserId' adında yeni bir özellik takıyorum!"
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            // Kullanıcının kimliğinden (Token içinden) ID bilgisini ara
            var userIdString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Eğer bulamazsan (adamın token'ı geçersizse veya yoksa) sistemi durdur! Middleware (Hata yakalayıcı) bunu tutacak.
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new BusinessException("Kimlik doğrulama hatası! Geçerli bir token bulunamadı!");
            }

            // Bulursan sayıya (int) çevir ve yolla!
            return Convert.ToInt32(userIdString);
        }
    }
}
