using AutoMapper;
using RentACar.Dtos.CarDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    // Profile sınıfından miras aldığımıza dikkat et (AutoMapper kütüphanesinden gelir)
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            // 1. KURAL: Veritabanından gelen Car nesnesini, müşteriye gidecek CarResultDto'ya çevir
            CreateMap<Car, CarResultDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));


            // 2. KURAL: Kullanıcıdan gelen CarCreateDto'yu (içinde Id yok), veritabanına kaydedilecek Car nesnesine çevir
            CreateMap<CarCreateDto, Car>();

            // 3. KURAL: Kullanıcıdan gelen Update DTO'sunu, veritabanına gidecek Car nesnesine çevir
            CreateMap<CarUpdateDto, Car>();

            CreateMap<Car, CarDetailDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                // ForMember(hedef => hedef.BrandName, ayar => ayar.MapFrom(kaynak => kaynak.Brand.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name));
        }
    }
}
