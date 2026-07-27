using RentACar.Core.Entities;

namespace RentACar.Dtos.CustomerDtos
{
    public class CustomerAddDto : IDto
    {
        public int UserId { get; set; }
        public string NationalIdentity { get; set; } = null!;
        public int DrivinglicenseYear { get; set; }
    }
}
