using RentACar.Dtos.OperationClaimDtos;
using RentACar.Dtos.UserDtos;
using RentACar.Dtos.UserOperationClaimDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimViewModels
{
    public class UserOperationClaimUpdateViewModel : IUserOperationClaimDropdownsViewModel
    {
        public List<UserResultDto> Users { get; set; } = new List<UserResultDto>();
        public List<OperationClaimResultDto> Roles { get; set; } = new List<OperationClaimResultDto>();
        public UserOperationClaimUpdateDto UserOperationClaimUpdate { get; set; } = new UserOperationClaimUpdateDto();
        public int UserId { get; set; }
    }   
}