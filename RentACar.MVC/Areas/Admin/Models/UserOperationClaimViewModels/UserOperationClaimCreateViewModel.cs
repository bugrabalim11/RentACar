using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimViewModels
{
    public class UserOperationClaimCreateViewModel 
    {
        // 3. Müşterinin (Admin'in) formda seçtiği ID'leri doldurup bize geri göndereceği boş sepet!
        // 'new' diyerek sepeti baştan yaratıyoruz ki, Vitrin (HTML) yüklenirken "içi boş sepete ulaşmaya
        // çalıştın" (NullReferenceException) bombası patlamasın.
        public UserOperationClaimCreateDto UserOperationClaimCreate { get; set; } = new UserOperationClaimCreateDto();
    }
}
