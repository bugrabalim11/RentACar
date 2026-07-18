using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dtos.ContactMessageDtos
{
    public class ContactMessageAddDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
