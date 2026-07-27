using RentACar.Core.Entities;

namespace RentACar.Dtos.CustomerDtos
{
    public class CustomerUpdateDto : IDto
    {
        public int Id { get; set; }
        public string NationalIdentity { get; set; } = null!;
        public int DrivinglicenseYear { get; set; }
    }
}
