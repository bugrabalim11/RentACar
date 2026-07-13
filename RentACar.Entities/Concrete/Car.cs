using RentACar.Core.Entities;
using RentACar.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class Car : IEntity
    {
        // Temel Özellikler
        public int Id { get; set; }
        public int BrandId { get; set; }

        // Yabancı Anahtar (Foreign Key)
        public int ColorId { get; set; }
        // Navigasyon Özelliği (Müfettişi susturmayı unutmuyoruz)
        public Color Color { get; set; } = null!;

        public int Kilometer { get; set; }
        public string ModelName { get; set; } = null!;
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }

        // Arayüzden (UI) Gelen Yeni Özellikler
        public int DoorCount { get; set; }
        public int SeatCount { get; set; }
        public int MinDriverAge { get; set; }
        public string LuggageCapacity { get; set; } = null!;
        public TransmissionType TransmissionType { get; set; }


        // --- İLİŞKİ (Bire-Çok) ---
        // Bir arabanın birden çok kiralama kaydı olabilir
        public List<Rental> Rentals { get; set; } = new List<Rental>();

        // Senin içinde tuttuğun o BrandId numarası öylesine bir sayı değil, fiziksel bir markayı temsil ediyor.
        public Brand Brand { get; set; } = null!;
    }
}
