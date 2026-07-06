using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class Customer
    {
        public int Id { get; set; }
        public int DrivingLicenseYear { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NationalIdentity { get; set; } = string.Empty;


        // --- İLİŞKİ (Bire-Çok) ---
        public List<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
