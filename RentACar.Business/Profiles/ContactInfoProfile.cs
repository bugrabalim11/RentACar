using AutoMapper;
using RentACar.Dtos.ContactInfoDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Profiles
{
    public class ContactInfoProfile : Profile
    {
        public ContactInfoProfile()
        {
            CreateMap<ContactInfoCreateDto, ContactInfo>();
            CreateMap<ContactInfoUpdateDto, ContactInfo>();
            CreateMap<ContactInfo, ContactInfoResultDto>();
        }
    }
}
