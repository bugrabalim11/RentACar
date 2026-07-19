using RentACar.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace RentACar.Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
    }
}
