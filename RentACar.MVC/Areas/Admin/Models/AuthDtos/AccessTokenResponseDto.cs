namespace RentACar.MVC.Areas.Admin.Models.AuthDtos
{
    public class AccessTokenResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
