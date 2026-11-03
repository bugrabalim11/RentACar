using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class Brand : BaseEntity, IEntity
    {
        public string Name { get; set; } = null!;
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
