using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Dtos.RentalDtos
{
    public class RentalListDto
    {
        public int Id { get; set; }
        public string CarName { get; set; } = null!;
        public string CustomerFirstName { get; set; } = null!;
        public string CustomerLastName { get; set; } = null!;
        public string PickUpOfficeName { get; set; } = null!;
        public string DropOffOfficeName { get; set; } = null!;
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
