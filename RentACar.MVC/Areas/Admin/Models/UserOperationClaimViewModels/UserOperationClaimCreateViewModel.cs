using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimViewModels
{
    public class UserOperationClaimCreateViewModel 
    {
        public UserOperationClaimCreateDto UserOperationClaimCreate { get; set; } = new UserOperationClaimCreateDto();
    }
}
