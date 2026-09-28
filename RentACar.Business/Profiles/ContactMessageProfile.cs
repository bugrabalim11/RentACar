using AutoMapper;
using RentACar.Dtos.ContactMessageDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class ContactMessageProfile : Profile
    {
        public ContactMessageProfile()
        {
            CreateMap<ContactMessage, ContactMessageResultDto>()
                .ForMember(dest => dest.SendDate, opt => opt.MapFrom(src => src.CreatedDate));
            CreateMap<ContactMessageCreateDto, ContactMessage>();
        }
    }
}
