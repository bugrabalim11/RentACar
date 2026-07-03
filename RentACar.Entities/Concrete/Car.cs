using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class Car
    {
        // Temel Özellikler
        public int Id { get; set; }
        public int BandId { get; set; }
        public int Kilometer { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }

        // Arayüzden (UI) Gelen Yeni Özellikler
        public int DoorCount { get; set; }
        public int SeatCount { get; set; }
        public int MiniDriverAge { get; set; }
        public string LagguageCapacity { get; set; } = string.Empty;
        public string TransmissionType { get; set; } = string.Empty;
    }
}
