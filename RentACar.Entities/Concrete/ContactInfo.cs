using RentACar.Core.Entities;
using RentACar.Core.Entities.Concrete;

namespace RentACar.Entities.Concrete
{
    public class ContactInfo : BaseEntity, IEntity
    {
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
