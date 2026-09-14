using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface IUserDropdownViewModel
    {
        List<OperationClaimResultDto> Roles { get; set; }
    }
}
