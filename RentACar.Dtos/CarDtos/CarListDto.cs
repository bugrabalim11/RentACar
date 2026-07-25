using RentACar.Core.Entities;
using RentACar.Entities.Enums;

namespace RentACar.Dtos.CarDtos
{
    public class CarListDto : IDto
    {
        public int Id { get; set; } 
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public int Kilometer { get; set; }
        public decimal DailyPrice { get; set; }
    }
}
