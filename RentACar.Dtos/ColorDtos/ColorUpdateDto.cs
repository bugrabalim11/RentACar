using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace RentACar.Dtos.ColorDtos
{
    public class ColorUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
