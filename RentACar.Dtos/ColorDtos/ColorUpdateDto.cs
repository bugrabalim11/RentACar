using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.ColorDtos
{
    public class ColorUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
