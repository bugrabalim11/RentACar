using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimDtos
{
    public class UserOperationClaimCreateViewModel : IUserOperationClaimDropdownsViewModel
    {
        public List<UserResultForAdminDto> Users { get; set; } = null!;
        public List<OperationClaimResultDto> Roles { get; set; } = null!;
    }
}
