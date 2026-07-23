using RentACar.Core.Entities;

namespace RentACar.Dtos.BrandDtos
{
    public class BrandListDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
