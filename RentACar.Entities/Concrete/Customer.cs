using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class Customer : BaseEntity, IEntity
    {
        public int DrivingLicenseYear { get; set; }
        public string NationalIdentity { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
