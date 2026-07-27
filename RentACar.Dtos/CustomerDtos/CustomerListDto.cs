using RentACar.Core.Entities;

namespace RentACar.Dtos.CustomerDtos
{
    public class CustomerListDto : IDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string NationalIdentity { get; set; } = null!;
        public int DrivingLicenseYear { get; set; }
    }
}
