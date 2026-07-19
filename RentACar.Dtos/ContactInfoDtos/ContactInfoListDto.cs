using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dtos.ContactInfoDtos
{
    public class ContactInfoListDto
    {
        public int Id { get; set; }
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
