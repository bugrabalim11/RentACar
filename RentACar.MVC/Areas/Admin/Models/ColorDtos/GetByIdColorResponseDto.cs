namespace RentACar.MVC.Areas.Admin.Models.ColorDtos
{
    public class GetByIdColorResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public ColorUpdateDto Data { get; set; } = null!;
    }
}
