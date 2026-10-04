using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.CustomerDtos
{
    public class CustomerCreateByAdminDto : IDto
    {
        public int UserId { get; set; }
        public string NationalIdentity { get; set; } = null!;
        public int DrivingLicenseYear { get; set; }
    }
}
