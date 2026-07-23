using RentACar.Core.Entities;

namespace RentACar.Dtos.ColorDtos
{
    public class ColorListDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
