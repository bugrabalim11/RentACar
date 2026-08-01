using AutoMapper;
using RentACar.Dtos.CarImageDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class CarImageProfile : Profile
    {
        public CarImageProfile()
        {
            CreateMap<CarImageAddDto, CarImage>();
            CreateMap<CarImageUpdateDto, CarImage>();
            CreateMap<CarImage, CarImageDetailDto>()
                .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => $"{src.Car.Brand.Name} {src.Car.ModelName}"));
        }
    }
}
