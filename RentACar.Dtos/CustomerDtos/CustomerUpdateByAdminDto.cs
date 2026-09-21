using RentACar.Core.Entities;

namespace RentACar.Dtos.CustomerDtos
{
    public class CustomerUpdateByAdminDto : IDto
    {
        public int Id { get; set; }
        public string NationalIdentity { get; set; } = null!;
        public int DrivingLicenseYear { get; set; }
    }
}
