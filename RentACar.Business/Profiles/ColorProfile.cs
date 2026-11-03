using AutoMapper;
using RentACar.Dtos.ColorDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class ColorProfile:Profile
    {
        public ColorProfile()
        {
            CreateMap<ColorCreateDto, Color>();
            CreateMap<ColorUpdateDto, Color>();
            CreateMap<Color, ColorResultDto>();
        }
    }
}
