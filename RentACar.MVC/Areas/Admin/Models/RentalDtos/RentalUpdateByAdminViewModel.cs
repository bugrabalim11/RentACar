using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.CustomerDtos;
using RentACar.MVC.Areas.Admin.Models.OfficeDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.RentalDtos
{
    public class RentalUpdateByAdminViewModel : IRentalDropdownsViewModel
    {
        [ValidateNever]
        public List<CustomerResultDto> Customers { get; set; } = new List<CustomerResultDto>();

        [ValidateNever]
        public List<CarResultDto> Cars { get; set; } = new List<CarResultDto>();

        [ValidateNever]
        public List<OfficeResultDto> Offices { get; set; } = new List<OfficeResultDto>();
        public RentalUpdateByAdminDto RentalUpdateByAdmin { get; set; } = new RentalUpdateByAdminDto();
    }
}