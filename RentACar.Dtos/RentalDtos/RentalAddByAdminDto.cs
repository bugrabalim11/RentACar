using RentACar.Core.Entities;

namespace RentACar.Dtos.RentalDtos
{
    public class RentalAddByAdminDto : IDto
    {
        public int CustomerId { get; set; }
        public int CarId { get; set; }
        public int PickUpOfficeId { get; set; }
        public int DropOffOfficeId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
