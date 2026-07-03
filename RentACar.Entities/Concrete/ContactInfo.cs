using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
