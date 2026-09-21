using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.UserDtos
{
    public class UserUpdateByAdminViewModel : IUserDropdownViewModel
    {
        [ValidateNever]
        public List<OperationClaimResultDto> Roles { get; set; } = null!;
        public UserUpdateByAdminDto UserUpdate { get; set; } = null!;
    }
}
