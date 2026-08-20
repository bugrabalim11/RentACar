using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CreditCardInformationDtos;

namespace RentACar.Business.Abstract
{
    /// <summary>
    /// Sistemdeki tüm ödeme işlemlerinin (Sahte POS vb.) geçeceği dış servis sözleşmesidir.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Kredi kartından belirtilen tutarı çekme işlemini simüle eder.
        /// </summary>
        /// <param name="creditCard">Müşterinin kredi kartı bilgileri</param>
        /// <param name="amount">Çekilecek toplam kiralama tutarı</param>
        /// <returns>İşlemin başarı durumunu (IResult) döner</returns>
        Task<IResult> PayAsync(CreditCardInformationDto creditCard, decimal amount);
    }
}
