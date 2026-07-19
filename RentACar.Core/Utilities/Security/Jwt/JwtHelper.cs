using RentACar.Core.Entities.Concrete;
using Microsoft.Extensions.Configuration; // IConfiguration için bu kütüphaneyi (ampul ile) eklemelisin.
using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }
    }
}
