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
            CreateMap<Rental, RentalDetailDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Car.Brand.Name))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Car.ModelName))
                .ForMember(dest => dest.MinDrivingExperience, opt => opt.MapFrom(src => src.Car.MinDrivingExperience))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Customer.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Customer.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Customer.User.Email))
                .ForMember(dest => dest.Plate, opt => opt.MapFrom(src => src.Car.Plate))
                .ForMember(dest => dest.DailyPrice, opt => opt.MapFrom(src => src.Car.DailyPrice))
                .ForMember(dest => dest.PickUpOfficeName, opt => opt.MapFrom(src => src.PickUpOffice.Name))
                .ForMember(dest => dest.DropOffOfficeName, opt => opt.MapFrom(src => src.DropOffOffice.Name));
            CreateMap<RentalAddDto, Rental>();
            CreateMap<RentalAddByAdminDto, Rental>();
            CreateMap<RentalUpdateDto, Rental>();
        }
    }
}
