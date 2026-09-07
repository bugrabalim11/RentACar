using AutoMapper;
using RentACar.Dtos.CarMaintenanceDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Profiles
{
    public class CarMaintenanceProfile : Profile
    {
        public CarMaintenanceProfile()
        {
            CreateMap<CarMaintenanceCreateDto, CarMaintenance>();
            CreateMap<CarMaintenanceUpdateDto, CarMaintenance>();
            CreateMap<CarMaintenance, CarMaintenanceResultDto>()
                .ForMember(dest => dest.CarBrandModel, opt => opt.MapFrom(src => $"{src.Car.Brand.Name} {src.Car.ModelName}"));
        }
    }
}
