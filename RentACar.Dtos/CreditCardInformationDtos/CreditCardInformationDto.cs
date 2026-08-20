using RentACar.Core.Entities;

namespace RentACar.Dtos.CreditCardInformationDtos
{
    /// <summary>
    /// Müşterinin ödeme ekranında gireceği kredi kartı bilgilerini taşıyan kargo nesnesidir.
    /// </summary>
    public class CreditCardInformationDto : IDto
    {
        public string CardHolderFullName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public int ExpireYear { get; set; }
        public int ExpireMonth { get; set; }
        public string Cvv { get; set; } = null!;
    }
}
