using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Utilities.Security.Jwt
{
    public class AccessToken
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
