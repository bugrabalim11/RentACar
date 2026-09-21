using RentACar.Core.Entities;
using RentACar.Dtos.CreditCardInformationDtos;

namespace RentACar.Dtos.RentalDtos
{
    public class RentalUpdateReturnDateDto : IDto
    {
        public DateTime ReturnDate { get; set; }
        public CreditCardInformationDto CreditCardInformation { get; set; } = null!;
    }
}
