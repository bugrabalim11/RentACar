using AutoMapper;
using RentACar.Dtos.CarMaintenanceDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class CarMaintenanceProfile : Profile
    {
        public CarMaintenanceProfile()
        {
            CreateMap<CarMaintenanceAddDto, CarMaintenance>();
            CreateMap<CarMaintenanceUpdateDto, CarMaintenance>();
            CreateMap<CarMaintenance, CarMaintenanceListDto>()
                .ForMember(dest => dest.CarBrandModel, opt => opt.MapFrom(src => $"{src.Car.Brand.Name} {src.Car.ModelName}"));
        }
    }
}
