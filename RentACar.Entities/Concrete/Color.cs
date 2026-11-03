using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class Color : BaseEntity, IEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
