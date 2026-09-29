using RentACar.Core.Entities;

namespace RentACar.Dtos.PaymentDtos
{
    public class PaymentCreateDto : IDto
    {
        public int RentalId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; } = null!;
        public bool IsSuccessful { get; set; }

        // Id, CreatedDate, UpdatedDate YOK! Onları Gümrük Memuru ve SQL halledecek!
    }
}
