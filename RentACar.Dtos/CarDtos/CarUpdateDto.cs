using RentACar.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dtos.CarDtos
{
    public class CarUpdateDto
    {
        public int Id { get; set; } 
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public string ModelName { get; set; } = null!;
        public int Kilometer { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int DoorCount { get; set; }
        public int SeatCount { get; set; }
        public string LuggageCapacity { get; set; } = null!;
        public TransmissionType TransmissionType { get; set; }
        public int MinDriverAge { get; set; }
    }
}
