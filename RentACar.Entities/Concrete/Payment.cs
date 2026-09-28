using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class Payment : BaseEntity, IEntity
    {
        public int RentalId { get; set; }

        // YANLIŞ: = new Rental(); (RAM'de boşuna araba üretme!)
        // DOĞRU: = null!; (SQL'den geleni işaret edeceğiz)
        public Rental Rental { get; set; } = null!;
        public decimal Amount { get; set; }
        public string TransactionId { get; set; } = null!;
        public bool IsSuccessful { get; set; } 
    }
}   
