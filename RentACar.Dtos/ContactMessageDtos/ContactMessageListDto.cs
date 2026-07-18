using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dtos.ContactMessageDtos
{
    public class ContactMessageListDto
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
