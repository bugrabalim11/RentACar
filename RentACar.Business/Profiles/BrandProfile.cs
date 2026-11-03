using AutoMapper;
using RentACar.Dtos.BrandDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<BrandCreateDto, Brand>();
            CreateMap<BrandUpdateDto, Brand>();
            CreateMap<Brand, BrandResultDto>();
        }
    }
}
