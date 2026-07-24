using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RentACar.API.Extensions
{
    public static class SecurityExtensions
    {
        // Uzantı metodumuz hem servisleri (IServiceCollection) hem de appsettings'i okuyabilmek için IConfiguration'ı almalı.
        public static void AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. ADIM: MAKİNEYİ ÇALIŞTIRMADAN ÖNCE KUTUYU KONTROL ET
            // Önce appsettings'den veriyi okumaya çalış, bulamazsan Exception (Hata) fırlatıp uygulamayı durdur!
            var securityKey = configuration["TokenOptions:SecurityKey"]
                ?? throw new ArgumentNullException("TokenOptions:SecurityKey", "appsettings.json içinde SecurityKey bulunamadı!");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = configuration["TokenOptions:Issuer"],
                  ValidAudience = configuration["TokenOptions:Audience"],

                  // 3. ADIM: MAKİNEYE DOLU OLDUĞUNDAN EMİN OLDUĞUMUZ KUTUYU (securityKey) VERİYORUZ
                  // Artık 'securityKey' değişkeninin boş (null) olmadığından %100 eminiz. C# da rahatladı.
                  // Şimdi GetBytes metodunun içine gönül rahatlığıyla verebiliriz.
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
              };
          });
        }
    }
}
