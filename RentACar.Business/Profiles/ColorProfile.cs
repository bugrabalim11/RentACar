using AutoMapper;
using RentACar.Dtos.ColorDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RentACar.Business.Profiles
{
    public class ColorProfile:Profile
    {
        public ColorProfile()
        {
            CreateMap<ColorAddDto, Color>();
            CreateMap<ColorUpdateDto, Color>();
            CreateMap<Color, ColorListDto>();
        }
    }
}
