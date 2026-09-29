using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.PaymentDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentManager(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<int>> AddAsync(PaymentCreateDto paymentCreateDto)
        {
            var payment = _mapper.Map<Payment>(paymentCreateDto);
            await _paymentRepository.AddAsync(payment);
            return new SuccessDataResult<int>(payment.Id, "Ödeme fişi başarıyla eklendi.");
        }
    }
}
