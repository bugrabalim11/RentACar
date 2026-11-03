using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RentACar.API.Extensions
{
    public static class SecurityExtensions
    {
        /// <summary>
        /// Projenin JWT (JSON Web Token) tabanlı kimlik doğrulama altyapısını kurar.
        /// AppSettings içerisindeki güvenlik anahtarlarını doğrulayarak sistemi başlatır.
        /// </summary>
        public static void AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
        {
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
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
              };
          });
        }
    }
}
