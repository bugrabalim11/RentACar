using RentACar.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class Brand : IEntity
    {
        public int Id { get; set; }

        // Ortada tabak falan yoktur. Sadece o kuralcı C# müfettişine dönüp
        // "Müfettiş bey, şu an tabak yok ama arka planda JSON kuryesi
        // veya EF Core nakliyecisi o tabağı birazdan getirecek, sen bana güven ve ceza yazma" demektir.
        public string Name { get; set; } = null!;

        // Sen bir markasın ve senin bünyende, senin ID'ni taşıyan bir sürü araba olacak
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
