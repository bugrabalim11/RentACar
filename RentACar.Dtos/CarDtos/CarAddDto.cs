using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dtos.CarDtos
{
    public class CarAddDto
    {
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public int Kilometer { get; set; }
        public string ModelName { get; set; } = null!;
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int DoorCount { get; set; }
        public int SeatCount { get; set; }
        public int MinDriverAge { get; set; }
        public string LuggageCapacity { get; set; } = null!;
        public string TransmissionType { get; set; } = null!;
    }
}
