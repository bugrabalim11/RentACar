using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class CarMaintenance : BaseEntity, IEntity
    {
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
