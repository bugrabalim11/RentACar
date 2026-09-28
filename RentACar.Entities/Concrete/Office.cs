using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class Office : BaseEntity, IEntity
    {
        // Id, CreatedDate, UpdatedDate, DeletedDate ve IsDeleted otomatik olarak BaseEntity'den (Babadan) gelir!
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
    }
}
