using RentACar.Core.Entities;

namespace RentACar.Dtos.ContactInfoDtos
{
    public class ContactInfoAddDto : IDto
    {
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
