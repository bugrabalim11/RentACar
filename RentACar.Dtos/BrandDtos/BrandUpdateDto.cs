using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.BrandDtos
{
    public class BrandUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
