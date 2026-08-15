using AutoMapper;
using RentACar.Dtos.CustomerDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerAddDto, Customer>();
            CreateMap<CustomerUpdateDto, Customer>();
            CreateMap<CustomerUpdateMyProfileDto, Customer>();

            CreateMap<Customer, CustomerListDto>()
                // DİKKAT: API sözleşmesini (DTO) veritabanı hiyerarşisine bağımlı kılmamak
                // adına AutoMapper'ın Flattening (Düzleştirme) özelliği bilerek kullanılmamıştır.
                // User tablosundaki isimler, DTO'ya manuel olarak (Explicit Mapping) haritalanmıştır.
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName));

            CreateMap<Customer, CustomerDetailDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.TotalRentals, opt => opt.MapFrom(src => src.Rentals.Count));
        }
    }
}
