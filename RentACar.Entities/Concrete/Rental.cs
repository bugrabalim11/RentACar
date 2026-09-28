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

        // --- İLİŞKİLER (NAVIGATION PROPERTIES) ---
        // Bu özellikler SQL'de sütun olmaz, EF Core'un Foreign Key kurmasını sağlar!
        public Car Car { get; set; } = null!;
        public Customer Customer { get; set; } = null!;

        // Ofis tablosuna iki farklı ilişki kurduğumuz için isimleri belirtiyoruz
        public Office PickUpOffice { get; set; } = null!;
        public Office DropOffOffice { get; set; } = null!;

        // Bire-Çok (One-to-Many) ilişki
        // Bu kiralamanın içinde ödemelerden oluşan bir KUTU (Liste) var" demektir.
        public List<Payment> Payments { get; set; } = new List<Payment>();
        // Koleksiyon (liste) oluştururken boş bir kutu yaratmak RAM'i yormaz,
        // aksine "Boş referans" (NullReferenceException) hatası yememizi engeller!
    }
}
