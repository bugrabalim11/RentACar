using RentACar.Core.Entities;

namespace RentACar.Dtos.ContactMessageDtos
{
    public class ContactMessageAddDto : IDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
