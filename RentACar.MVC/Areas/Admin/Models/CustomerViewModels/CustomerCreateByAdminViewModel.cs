using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.Dtos.CustomerDtos;
using RentACar.Dtos.UserDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.CustomerViewModels
{
    public class CustomerCreateByAdminViewModel : ICustomerDropdownViewModel
    {
        [ValidateNever]
        public List<UserResultDto> Users { get; set; } = new List<UserResultDto>();
        public CustomerCreateByAdminDto CustomerCreateByAdmin { get; set; } = new CustomerCreateByAdminDto();
    }
}
