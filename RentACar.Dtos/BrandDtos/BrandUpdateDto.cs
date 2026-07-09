using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dtos.BrandDtos
{
    public class BrandUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
