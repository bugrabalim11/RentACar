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
            CreateMap<CarMaintenance, CarMaintenanceListDto>();
        }
    }
}
