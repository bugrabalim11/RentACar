using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class CarImage : BaseEntity, IEntity
    {
        // Id, CreatedDate, UpdatedDate, DeletedDate ve IsDeleted otomatik olarak BaseEntity'den (Babadan) gelir!
        public int CarId { get; set; }
        public string ImagePath { get; set; } = null!;
        public Car Car { get; set; } = null!;
    }
}
