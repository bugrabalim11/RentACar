using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimDtos
{
    public class UserOperationClaimUpdateViewModel : IUserOperationClaimDropdownsViewModel
    {
        public List<UserResultDto> Users { get; set; } = new List<UserResultDto>();
        public List<OperationClaimResultDto> Roles { get; set; } = new List<OperationClaimResultDto>();
        public UserOperationClaimUpdateDto UserOperationClaimUpdate { get; set; } = new UserOperationClaimUpdateDto();
    }
}
