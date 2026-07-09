using AutoMapper;
using RentACar.Dtos.BrandDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Profiles
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<BrandAddDto, Brand>();
            CreateMap<BrandUpdateDto, Brand>();
            CreateMap<Brand, BrandListDto>().ReverseMap();
        }
    }
}
