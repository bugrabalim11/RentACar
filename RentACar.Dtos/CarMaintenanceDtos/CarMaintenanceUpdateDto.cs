using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.CarMaintenanceDtos
{
    public class CarMaintenanceUpdateDto:IDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
