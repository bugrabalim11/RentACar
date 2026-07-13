using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dtos.CustomerDtos
{
    public class CustomerListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string NationalIdentity { get; set; } = null!;
        public int DrivinglicenseYear { get; set; }
    }
}
