using RentACar.Core.Entities;
using RentACar.Entities.Enums;

namespace RentACar.Dtos.CarDtos
{
    public class CarDetailDto : IDto
    {
        public int Id { get; set; }

        // Müşteri ID görmek istemez, ismi görmek ister!
        public string BrandName { get; set; } = null!;
        public string ColorName { get; set; } = null!;

        // Senin yazdığın detaylar aynen kalıyor:
        public string ModelName { get; set; } = null!;
        public int Kilometer { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int DoorCount { get; set; }
        public int SeatCount { get; set; }
        public LuggageCapacity LuggageCapacity { get; set; }
        public string TransmissionType { get; set; } = null!;
        public int MinDriverAge { get; set; }
        public int MinDrivingExperience { get; set; }
        public int MinFindexScore { get; set; }
    }
}
