using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Areas.Admin.Models.UserDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface IUserOperationClaimDropdownsViewModel
    {
        List<UserResultDto> Users {  get; set; }
        List<OperationClaimResultDto> Roles {  get; set; }
    }
}
