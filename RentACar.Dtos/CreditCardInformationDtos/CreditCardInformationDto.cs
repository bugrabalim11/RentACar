using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.CreditCardInformationDtos
{
    public class CreditCardInformationDto : IDto
    {
        public string CardHolderFullName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public int ExpireYear { get; set; }
        public int ExpireMonth { get; set; }
        public string Cvv { get; set; } = null!;
    }
}
