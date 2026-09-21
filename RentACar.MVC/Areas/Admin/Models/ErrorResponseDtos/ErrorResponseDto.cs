namespace RentACar.MVC.Areas.Admin.Models.ErrorResponseDtos
{
    public class ErrorResponseDto
    {
        // Römork (Data) yok! Sadece başarı durumu ve mesaj var.
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}
