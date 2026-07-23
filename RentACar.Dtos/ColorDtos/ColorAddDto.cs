using RentACar.Core.Entities;

namespace RentACar.Dtos.ColorDtos
{
    public class ColorAddDto : IDto
    {
        public string Name { get; set; } = null!;
    }
}
