using AutoMapper;
using RentACar.Dtos.OfficeDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            CreateMap<OfficeCreateDto, Office>();
            CreateMap<OfficeUpdateDto, Office>();
            CreateMap<Office, OfficeResultDto>();
        }
    }
}
