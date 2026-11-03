using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CreditCardInformationDtos;

namespace RentACar.Business.Abstract
{
    /// <summary>
    /// Sistemdeki tüm ödeme işlemlerinin (Sahte POS vb.) geçeceği dış servis sözleşmesidir.
    /// </summary>
    public interface IPosService
    {
        Task<IResult> PayAsync(CreditCardInformationDto creditCard, decimal amount);
    }
}
