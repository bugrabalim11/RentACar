using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;
using RentACar.Entities.Enums;

namespace RentACar.Entities.Concrete
{
    public class Car : BaseEntity, IEntity
    {
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public int ColorId { get; set; }
        public Color Color { get; set; } = null!;
        public int Kilometer { get; set; }
        public string ModelName { get; set; } = null!;
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int DoorCount { get; set; }
        public int SeatCount { get; set; }
        public int MinDriverAge { get; set; }
        public LuggageCapacity LuggageCapacity { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public int MinDrivingExperience { get; set; }
        public int MinFindexScore { get; set; }
        public List<Rental> Rentals { get; set; } = new List<Rental>();
        public List<CarImage> CarImages { get; set; } = new List<CarImage>();
    }
}
