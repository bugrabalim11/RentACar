using RentACar.Core.Entities;
using RentACar.Dtos.CreditCardInformationDtos;

namespace RentACar.Dtos.RentalDtos
{
    public class RentalAddDto : IDto
    {
        public int CarId { get; set; }
        public int PickUpOfficeId { get; set; }
        public int DropOffOfficeId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public CreditCardInformationDto CreditCardInformation { get; set; } = null!;
    }
}
