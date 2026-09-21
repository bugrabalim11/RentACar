using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.CustomerDtos
{
    public class CustomerCreateByAdminViewModel : ICustomerDropdownViewModel
    {
        [ValidateNever]
        public List<UserResultDto> Users { get; set; } = new List<UserResultDto>();
        public CustomerCreateByAdminDto CustomerCreateByAdmin { get; set; } = new CustomerCreateByAdminDto();
    }
}
