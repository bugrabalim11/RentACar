namespace RentACar.MVC.Areas.Admin.Models.CarMaintenanceDtos
{
    public class GetByIdCarMaintenanceResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public CarMaintenanceUpdateDto Data { get; set; } = null!;
    }
}
