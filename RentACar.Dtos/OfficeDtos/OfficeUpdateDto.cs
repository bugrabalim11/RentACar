using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dtos.OfficeDtos
{
    public class OfficeUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
    }
}
