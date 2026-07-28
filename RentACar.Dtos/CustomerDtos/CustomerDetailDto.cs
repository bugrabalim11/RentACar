using RentACar.Core.Entities;

namespace RentACar.Dtos.CustomerDtos
{
    public class CustomerDetailDto : IDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string NationalIdentity { get; set; } = null!;
        public string DrivingLicenseYear { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int TotalRentals { get; set; }
    }
}
