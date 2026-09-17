using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimDtos
{
    public class UserOperationClaimCreateViewModel : IUserOperationClaimDropdownsViewModel
    {
        public List<UserResultDto> Users { get; set; } = new List<UserResultDto>();
        public List<OperationClaimResultDto> Roles { get; set; } = new List<OperationClaimResultDto>();

        // 3. Müşterinin (Admin'in) formda seçtiği ID'leri doldurup bize geri göndereceği boş sepet!
        // 'new' diyerek sepeti baştan yaratıyoruz ki, Vitrin (HTML) yüklenirken "içi boş sepete ulaşmaya
        // çalıştın" (NullReferenceException) bombası patlamasın.
        public UserOperationClaimCreateDto UserOperationClaimCreate { get; set; } = new UserOperationClaimCreateDto();
    }
}
