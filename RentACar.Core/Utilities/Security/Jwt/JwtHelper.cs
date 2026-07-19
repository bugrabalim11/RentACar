using RentACar.Core.Entities.Concrete;
using Microsoft.Extensions.Configuration; // IConfiguration için bu kütüphaneyi (ampul ile) eklemelisin.
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace RentACar.Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        // 1. Gizli depolarımız (Kablolarımız)
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;


        // 2. Constructor (Tesisatçının kabloyu taktığı an!)
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            // Kablodan (appsettings.json) gelen veriyi bizim TokenOptions şablonumuza dolduruyoruz.
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>() ?? new TokenOptions();
        }

        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            // 1. Bilekliğe yazılacak temel bilgileri (İddiaları) hazırlıyoruz.
            var claims = new List<Claim>
            {
                // NameIdentifier (Kullanıcının ID'si)
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),

                // Email adresi
                new Claim(ClaimTypes.Email,user.Email),

                // Adı ve Soyadı
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            // 2. Kullanıcının rollerini (Admin, Müşteri vb.) bilekliğe tek tek ekliyoruz.
            // İşte Senior farkı! Tek rol değil, liste halinde roller geliyor.
            foreach (var role in operationClaims)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            // 2. MÜHÜR HAZIRLIĞI (Credentials)
            // String şifreyi byte dizisine (demire) çevirip mühür kalıbı yapıyoruz.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            // Mührü HmacSha256 mürekkebine batırıp yetki belgesini (Credentials) oluşturuyoruz.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. BASKI MAKİNESİ (Token'ı Üretmek)
            var jwtToken = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration),
                claims: claims,
                signingCredentials: credentials
            );

            // 4. BİLEKLİĞİ MÜŞTERİYE TESLİM ET (Return)
            // Üretilen nesneyi WriteToken ile uzun bir barkod string'ine çeviriyoruz.
            return new AccessToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Expiration = jwtToken.ValidTo
            };
        }
    }
}
