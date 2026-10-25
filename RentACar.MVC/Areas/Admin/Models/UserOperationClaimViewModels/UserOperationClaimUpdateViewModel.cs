using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimViewModels
{
    public class UserOperationClaimUpdateViewModel 
    {
        public UserOperationClaimUpdateDto UserOperationClaimUpdate { get; set; } = new UserOperationClaimUpdateDto();
        public int UserId { get; set; }
    }   
}