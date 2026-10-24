using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.Dtos.CarDtos;
using RentACar.Dtos.CustomerDtos;
using RentACar.Dtos.OfficeDtos;
using RentACar.Dtos.RentalDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.RentalViewModels
{
    public class RentalCreateByAdminViewModel : IRentalDropdownsViewModel
    {
        [ValidateNever]
        public List<CustomerResultDto> Customers { get; set; } = new List<CustomerResultDto>();

        [ValidateNever]
        public List<CarResultDto> Cars { get; set; } = new List<CarResultDto>();

        [ValidateNever]
        public List<OfficeResultDto> Offices { get; set; } = new List<OfficeResultDto>();
        public RentalCreateByAdminDto RentalCreateByAdmin { get; set; } = new RentalCreateByAdminDto();
    }
}
