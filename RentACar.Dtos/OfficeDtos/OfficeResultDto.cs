using RentACar.Core.Entities;

namespace RentACar.Dtos.OfficeDtos
{
    public class OfficeResultDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
    }
}
