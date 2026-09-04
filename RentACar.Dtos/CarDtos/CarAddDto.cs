
using RentACar.Core.Entities;
using RentACar.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentACar.Dtos.CarDtos
{
    public class CarAddDto : IDto
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
        public LuggageCapacity LuggageCapacity { get; set; }
        public int MinDrivingExperience { get; set; }
        public int MinFindexScore { get; set; }
        public TransmissionType TransmissionType { get; set; }
    }
}
