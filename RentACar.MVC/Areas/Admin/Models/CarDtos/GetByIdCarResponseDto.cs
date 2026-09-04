namespace RentACar.MVC.Areas.Admin.Models.CarDtos
{
    public class GetByIdCarResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public CarUpdateDto Data { get; set; } = null!;
    }
}
