namespace RentACar.MVC.Areas.Admin.Models.CarImageDtos
{
    public class CarImageResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<CarImageResultDto> Data { get; set; } = null!;
    }
}
