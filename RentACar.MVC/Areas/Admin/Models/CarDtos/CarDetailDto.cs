using RentACar.MVC.Models.Enums;

namespace RentACar.MVC.Areas.Admin.Models.CarDtos
{
    public class CarDetailDto
    {
        public int Id { get; set; }

        // Müşteri ID görmek istemez, ismi görmek ister!
        public string BrandName { get; set; } = null!;
        public string ColorName { get; set; } = null!;

        public string ModelName { get; set; } = null!;
        public int Kilometer { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int DoorCount { get; set; }
        public int SeatCount { get; set; }
        public LuggageCapacity LuggageCapacity { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public int MinDriverAge { get; set; }
        public int MinDrivingExperience { get; set; }
        public int MinFindexScore { get; set; }
    }
}
