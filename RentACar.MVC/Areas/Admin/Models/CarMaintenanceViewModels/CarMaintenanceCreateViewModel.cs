using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.MVC.Areas.Admin.Models.CarMaintenanceViewModels
{
    public class CarMaintenanceCreateViewModel 
    {
        public CarMaintenanceCreateViewModel()
        {

            CarMaintenanceCreate = new CarMaintenanceCreateDto();
        }

        [ValidateNever]
        public CarMaintenanceCreateDto CarMaintenanceCreate { get; set; } = null!;
    }
}
