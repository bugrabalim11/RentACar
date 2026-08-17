using RentACar.Core.Entities;

namespace RentACar.Dtos.CustomerDtos
{
    public class CustomerAddDto : IDto
    {
        public string NationalIdentity { get; set; } = null!;
        public int DrivingLicenseYear { get; set; }
    }
}
