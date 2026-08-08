using AutoMapper;
using RentACar.Dtos.RentalDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            // CreateMap<Kaynak, Hedef>

            CreateMap<Rental, RentalListDto>()
                // Hedefteki (dest) CustomerFirstName alanına, kaynaktaki (src) Customer.User.FirstName alanını haritala (MapFrom).
                .ForMember(dest => dest.CustomerFirstName, opt => opt.MapFrom(src => src.Customer.User.FirstName))
                .ForMember(dest => dest.CustomerLastName, opt => opt.MapFrom(src => src.Customer.User.LastName));

            CreateMap<RentalAddDto, Rental>();
            CreateMap<RentalUpdateDto, Rental>();
        }
    }
}
