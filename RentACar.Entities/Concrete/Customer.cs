using RentACar.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class Customer : IEntity
    {
        public int Id { get; set; }
        public int DrivingLicenseYear { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string NationalIdentity { get; set; } = null!;
        public bool Status { get; set; } = true;


        // --- İLİŞKİ (Bire-Çok) ---
        public List<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
