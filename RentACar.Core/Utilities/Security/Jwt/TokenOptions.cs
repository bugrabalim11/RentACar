namespace RentACar.Core.Utilities.Security.Jwt
{
    /// <summary>
    /// appsettings.json içerisindeki JWT (JSON Web Token) konfigürasyonlarını karşılayan modeldir.
    /// </summary>
    public class TokenOptions
    {
        public string Audience { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public int AccessTokenExpiration { get; set; }
        public string SecurityKey { get; set; } = null!;
    }
}
