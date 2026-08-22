using RentACar.Core.Entities;

namespace RentACar.Dtos.CarMaintenanceDtos
{
    public class CarMaintenanceListDto:IDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarBrandModel { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
