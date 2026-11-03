namespace RentACar.Core.Utilities.Security.Jwt
{
    /// <summary>
    /// İstemciye (Client) gönderilecek olan yetkilendirme anahtarı (Token) ve geçerlilik süresi (Expiration) bilgisini taşır.
    /// </summary>
    public class AccessToken
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
