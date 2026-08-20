using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CreditCardInformationDtos;

namespace RentACar.Business.Concrete
{
    /// <summary>
    /// Sistemdeki ödeme işlemlerini dış bir servise (Örn: Iyzico, Stripe, Garanti POS)
    /// gidiyormuş gibi simüle eden sahte (Fake) banka yöneticisidir.
    /// </summary>
    public class PaymentManager : IPaymentService
    {
        public async Task<IResult> PayAsync(CreditCardInformationDto creditCard, decimal amount)
        {
            await Task.Delay(1000);
            return new SuccessResult("Ödeme başarıyla alındı.");
        }
    }
}
