using RentACar.MVC.Areas.Admin.Models.CarDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface ICarMaintenanceDropdownViewModel
    {
        List<CarResultDto> Cars { get; set; }
    }
}
