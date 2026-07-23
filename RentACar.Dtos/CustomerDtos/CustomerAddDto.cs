using RentACar.Core.Entities;

namespace RentACar.Dtos.CustomerDtos
{
    public class CustomerAddDto : IDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string NationalIdentity { get; set; } = null!;
        public int DrivinglicenseYear { get; set; }
    }
}
