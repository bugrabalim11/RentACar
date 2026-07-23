using RentACar.Core.Entities;

namespace RentACar.Dtos.BrandDtos
{
    public class BrandAddDto : IDto
    {
        public string Name { get; set; } = null!;
    }
}
