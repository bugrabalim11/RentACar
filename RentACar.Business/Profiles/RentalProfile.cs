using AutoMapper;
using RentACar.Dtos.RentalDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Profiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            // CreateMap<Kaynak, Hedef>

            CreateMap<Rental, RentalListDto>();
            CreateMap<RentalAddDto, Rental>();
            CreateMap<RentalUpdateDto, Rental>();
        }
    }
}
