using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.BrandDtos
{
    public class BrandCreateDto : IDto
    {
        public string Name { get; set; } = null!;
    }
}
