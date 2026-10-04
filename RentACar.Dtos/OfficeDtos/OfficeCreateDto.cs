using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.OfficeDtos
{
    public class OfficeCreateDto : IDto
    {
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
    }
}
