using RentACar.Core.Utilities.Results;
using RentACar.Dtos.PaymentDtos;

namespace RentACar.Business.Abstract
{
    public interface IPaymentService
    {
        Task<IDataResult<int>> AddAsync(PaymentCreateDto paymentCreateDto);
    }
}
