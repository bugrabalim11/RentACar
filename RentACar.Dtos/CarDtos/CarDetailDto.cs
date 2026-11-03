using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.CarDtos
{
    public class CarDetailDto : IDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public string BrandName { get; set; } = null!;
        public string ColorName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public int Kilometer { get; set; }
        public string Plate { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int DoorCount { get; set; }
        public int SeatCount { get; set; }
        public string LuggageCapacity { get; set; } = null!;
        public string TransmissionType { get; set; } = null!;
        public int MinDriverAge { get; set; }
        public int MinDrivingExperience { get; set; }
        public int MinFindexScore { get; set; }
        public List<string> CoverImagesUrl { get; set; } = null!;
    }
}
