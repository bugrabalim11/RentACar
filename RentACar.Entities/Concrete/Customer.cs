using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class Customer : IEntity
    {
        public int Id { get; set; }
        public int DrivingLicenseYear { get; set; }
        public string NationalIdentity { get; set; } = null!;
        public int UserId { get; set; }
        // bu ilişkinin sadece bir sayıdan ibaret olmadığını, gerçekten User tablosuna bağlandığını anlasın
        public User User { get; set; }=null!;
        public bool Status { get; set; } = true;


        // --- İLİŞKİ (Bire-Çok) ---
        public List<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
