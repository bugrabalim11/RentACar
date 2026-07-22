using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Entities.DTOs.AuthDtos
{
    public class UserForLoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
