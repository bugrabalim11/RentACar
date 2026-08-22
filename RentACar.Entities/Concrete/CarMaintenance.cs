using RentACar.Core.Entities;

namespace RentACar.Entities.Concrete
{
    public class CarMaintenance : IEntity
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CheckInTime { get; set; }
        public DateTime? CeheckOutTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
