namespace RentACar.MVC.Areas.Admin.Models.CarDtos
{
    public class CarDetailResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public CarDetailDto Data { get; set; } = null!;
    }
}
