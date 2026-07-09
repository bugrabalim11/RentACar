using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Sen bir markasın ve senin bünyende, senin ID'ni taşıyan bir sürü araba olacak
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
