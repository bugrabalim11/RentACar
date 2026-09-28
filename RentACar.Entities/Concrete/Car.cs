using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;
using RentACar.Entities.Enums;

namespace RentACar.Entities.Concrete
{
    public class Car : BaseEntity, IEntity
    {
        // Id, CreatedDate, UpdatedDate, DeletedDate ve IsDeleted otomatik olarak BaseEntity'den (Babadan) gelir!
        public int BrandId { get; set; }

        // Senin içinde tuttuğun o BrandId numarası öylesine bir sayı değil, fiziksel bir markayı temsil ediyor.
        public Brand Brand { get; set; } = null!;

        // Yabancı Anahtar (Foreign Key)
        public int ColorId { get; set; }
        // Navigasyon Özelliği (Müfettişi susturmayı unutmuyoruz)
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

        // --- İLİŞKİ (Bire-Çok) ---
        // Bir arabanın birden çok kiralama kaydı olabilir
        public List<Rental> Rentals { get; set; } = new List<Rental>();
        public List<CarImage> CarImages { get; set; } = new List<CarImage>();
    }
}
