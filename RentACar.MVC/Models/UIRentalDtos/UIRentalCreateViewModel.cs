using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.CustomerDtos;
using RentACar.MVC.Areas.Admin.Models.OfficeDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.UIRentalViewModels;

namespace RentACar.MVC.Models.UIRentalDtos
{
    public class UIRentalCreateViewModel : IRentalDropdownsViewModel
    // TODO Ana dizideki interfaceleri admin ve user olamk üzere ayır çünkü burda cars ve cutomer gereksiz
    {
        [ValidateNever]
        public List<CustomerResultDto> Customers { get; set; } = new List<CustomerResultDto>();

        [ValidateNever]
        public List<CarResultDto> Cars { get; set; } = new List<CarResultDto>();

        [ValidateNever]
        public List<OfficeResultDto> Offices { get; set; } = new List<OfficeResultDto>();
        public UIRentalCreateDto UIRentalCreateDto { get; set; } = new UIRentalCreateDto();
    }
}
