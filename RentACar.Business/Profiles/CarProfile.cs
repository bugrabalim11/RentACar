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
            // SENİOR NOTU:
            // EĞER src.CarImages.FirstOrDefault().ImagePath yazsaydık; arabanın hiç resmi olmadığında 
            // null bir nesnenin içinden ImagePath okumaya çalışacağımız için NullReferenceException (CS8602) alırdık.
            // Bu yüzden önce .Select() ile sadece etiketleri (yazıları) çektik.
            // Ayrıca UI (Frontend) tarafını if-else ile kirletmemek için (Clean Code), null gelme durumunda 
            // ?? operatörü ile varsayılan (default) bir resim atadık.
            CreateMap<Car, CarResultDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CarImages.Select(x => "https://localhost:7085/" + x.ImagePath).FirstOrDefault() ?? "/uiAssets/images/default-car.jpg"));


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
