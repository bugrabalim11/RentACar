namespace RentACar.MVC.Areas.Admin.Models.ColorDtos
{
    public class ColorResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<ColorResultDto> Data { get; set; } = null!;
    }
}
