using RentACar.Core.Entities.Concrete;

namespace RentACar.Core.Utilities.Security.Jwt
{
    /// <summary>
    /// Proje genelinde Token üretme operasyonlarını soyutlayan temel arayüzdür.
    /// </summary>
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
    }
}
