using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Bir rengin birden fazla arabası olabilir (One-to-Many ilişkisi)
        // Tıpkı Brand tablosunda yaptığımız gibi listemizi hazırlıyoruz:
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
