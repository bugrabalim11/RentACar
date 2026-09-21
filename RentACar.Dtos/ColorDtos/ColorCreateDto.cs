using RentACar.Core.Entities;

namespace RentACar.Dtos.ColorDtos
{
    public class ColorCreateDto : IDto
    {
        public string Name { get; set; } = null!;
    }
}
