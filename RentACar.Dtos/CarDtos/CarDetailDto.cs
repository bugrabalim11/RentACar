using RentACar.Core.Entities;
using RentACar.Entities.Enums;
using System.Runtime.Intrinsics.X86;

namespace RentACar.Dtos.CarDtos
{
    public class CarDetailDto : IDto
    {
        public int Id { get; set; }

        // AMA MANKİNE ID İSTER MVC'DEKİ DROPDOWN İÇİN RENGİN HANGİ ID YE AİT OLDUĞUNU BİLMEK İÇİN GEREKLİDİR. 
        public int BrandId { get; set; }
        public int ColorId { get; set; }

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
        public string LuggageCapacity { get; set; } = null!;
        public string TransmissionType { get; set; } = null!;
        public int MinDriverAge { get; set; }
        public int MinDrivingExperience { get; set; }
        public int MinFindexScore { get; set; }
    }
}
