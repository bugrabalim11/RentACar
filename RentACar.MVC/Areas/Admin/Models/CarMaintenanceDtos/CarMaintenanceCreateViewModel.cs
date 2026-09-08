using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.CarMaintenanceDtos
{
    public class CarMaintenanceCreateViewModel : ICarMaintenanceDropdownViewModel
    {
        [ValidateNever]
        public List<CarResultDto> Cars { get; set; } = null!;
        public CarMaintenanceCreateDto CarMaintenanceCreate { get; set; } = null!;
    }
}
