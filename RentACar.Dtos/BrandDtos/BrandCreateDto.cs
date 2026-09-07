using RentACar.Core.Entities;

namespace RentACar.Dtos.BrandDtos
{
    public class BrandCreateDto : IDto
    {
        public string Name { get; set; } = null!;
    }
}
