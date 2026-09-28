using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class Color : BaseEntity, IEntity
    {
        public string Name { get; set; } = null!;

        // Bir rengin birden fazla arabası olabilir (One-to-Many ilişkisi)
        // Tıpkı Brand tablosunda yaptığımız gibi listemizi hazırlıyoruz:
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
