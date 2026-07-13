using RentACar.Core.Entities;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace RentACar.Entities.Concrete
{
    public class ContactInfo : IEntity
    {
        public int Id { get; set; }
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
