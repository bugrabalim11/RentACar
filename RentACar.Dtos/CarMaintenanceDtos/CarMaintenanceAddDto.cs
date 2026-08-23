using RentACar.Core.Entities;

namespace RentACar.Dtos.CarMaintenanceDtos
{
    public class CarMaintenanceAddDto : IDto
    {
        public int CarId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
