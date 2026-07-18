using AutoMapper;
using RentACar.Dtos.ContactMessageDtos;
using RentACar.Dtos.RentalDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Profiles
{
    public class ContactMessageProfile : Profile
    {
        public ContactMessageProfile()
        {
            CreateMap<ContactMessage, ContactMessageListDto>();
            CreateMap<ContactMessageAddDto, ContactMessage>();
        }
    }
}
