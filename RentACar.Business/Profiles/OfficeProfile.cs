using AutoMapper;
using RentACar.Dtos.OfficeDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Profiles
{
    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            // Veritabanına yazılacaklar (DTO -> Entity)
            CreateMap<OfficeAddDto, Office>();
            CreateMap<OfficeUpdateDto, Office>();

            // Vitrine gönderilecekler (Entity -> DTO) Tek Yönlü!
            CreateMap<Office, OfficeListDto>();
        }
    }
}
