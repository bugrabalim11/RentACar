using RentACar.Dtos.OperationClaimDtos;
using RentACar.Dtos.UserDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface IUserOperationClaimDropdownsViewModel
    {
        List<UserResultDto> Users {  get; set; }
        List<OperationClaimResultDto> Roles {  get; set; }
    }
}
