using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class CarMaintenance : BaseEntity, IEntity
    {
        // Id, CreatedDate, UpdatedDate, DeletedDate ve IsDeleted otomatik olarak BaseEntity'den (Babadan) gelir!
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
