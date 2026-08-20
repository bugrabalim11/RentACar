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
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Customer.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Customer.User.LastName))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Car.Brand.Name))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Car.ModelName));

            CreateMap<RentalAddDto, Rental>();
            CreateMap<RentalAddByAdminDto, Rental>();
            CreateMap<RentalUpdateDto, Rental>();
        }
    }
}
