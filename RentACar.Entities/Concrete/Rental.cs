using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class Rental : BaseEntity, IEntity
    {
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public int PickUpOfficeId { get; set; }
        public int DropOffOfficeId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }

        public Car Car { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
        public Office PickUpOffice { get; set; } = null!;
        public Office DropOffOffice { get; set; } = null!;

        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
