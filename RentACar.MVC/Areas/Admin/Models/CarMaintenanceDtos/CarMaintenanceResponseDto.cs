namespace RentACar.MVC.Areas.Admin.Models.CarMaintenanceDtos
{
    public class CarMaintenanceResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<CarMaintenanceResultDto> Data { get; set; } = null!;
    }
}
