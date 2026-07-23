using RentACar.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class Rental : IEntity
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public int PickUpOfficeId { get; set; }
        public int DropOffOfficeId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool Status { get; set; } = true;



        // --- İLİŞKİLER (NAVIGATION PROPERTIES) ---
        // Bu özellikler SQL'de sütun olmaz, EF Core'un Foreign Key kurmasını sağlar!
        public Car Car { get; set; } = null!;
        public Customer Customer { get; set; } = null!;

        // Ofis tablosuna iki farklı ilişki kurduğumuz için isimleri belirtiyoruz
        public Office PickUpOffice { get; set; } = null!;
        public Office DropOffOffice { get; set; } = null!;
    }
}
