using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class Car
    {
        // Temel Özellikler
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int Kilometer { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }

        // Arayüzden (UI) Gelen Yeni Özellikler
        public int DoorCount { get; set; }
        public int SeatCount { get; set; }
        public int MinDriverAge { get; set; }
        public string LuggageCapacity { get; set; } = string.Empty;
        public string TransmissionType { get; set; } = string.Empty;


        // --- İLİŞKİ (Bire-Çok) ---
        // Bir arabanın birden çok kiralama kaydı olabilir
        public List<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
