using AutoMapper;
using RentACar.Dtos.PaymentDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentCreateDto, Payment>();
        }
    }
}
