using AutoMapper;
using RentACar.Dtos.CarDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{

    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarResultDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                // TODO: API URL' i hardcoded yazılı canlıya alırsak patlar
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CarImages.Select(x => "https://localhost:7085/" + x.ImagePath).FirstOrDefault() ?? "/uiAssets/images/default-car.jpg"));

            CreateMap<CarCreateDto, Car>();

            CreateMap<CarUpdateDto, Car>();

            CreateMap<Car, CarDetailDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.CoverImagesUrl, opt => opt.MapFrom(src => src.CarImages.Any()
                ? src.CarImages.Select(x => "https://localhost:7085/" + x.ImagePath).ToList()
                : new List<string> { "/uiAssets/images/default-car.jpg" }));
        }
    }
}
