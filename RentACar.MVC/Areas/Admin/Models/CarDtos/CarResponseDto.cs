namespace RentACar.MVC.Areas.Admin.Models.CarDtos
{
    public class CarResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<CarResultDto> Data { get; set; } = null!;
    }
}
