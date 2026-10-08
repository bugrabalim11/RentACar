using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.CarDtos
{
    public class CarResultDto : IDto
    {
        public int Id { get; set; } 
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public int Kilometer { get; set; }
        public decimal DailyPrice { get; set; }
        public string Plate { get; set; } = null!;
        public string CoverImageUrl { get; set; } = null!;  
    }
}
