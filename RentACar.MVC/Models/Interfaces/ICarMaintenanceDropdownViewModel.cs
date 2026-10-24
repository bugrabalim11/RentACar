using RentACar.Dtos.CarDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface ICarMaintenanceDropdownViewModel
    {
        List<CarResultDto> Cars { get; set; }
    }
}
