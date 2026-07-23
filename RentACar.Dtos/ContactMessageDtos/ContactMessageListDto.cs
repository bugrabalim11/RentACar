using RentACar.Core.Entities;

namespace RentACar.Dtos.ContactMessageDtos
{
    public class ContactMessageListDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
    }
}
