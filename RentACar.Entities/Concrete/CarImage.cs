using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class CarImage : BaseEntity, IEntity
    {
        public int CarId { get; set; }
        public string ImagePath { get; set; } = null!;
        public Car Car { get; set; } = null!;
    }
}
