using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.Dtos.OperationClaimDtos;
using RentACar.Dtos.UserDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.UserViewModels
{
    public class UserUpdateByAdminViewModel : IUserDropdownViewModel
    {
        [ValidateNever]
        public List<OperationClaimResultDto> Roles { get; set; } = null!;
        public UserUpdateByAdminDto UserUpdate { get; set; } = null!;
    }
}
