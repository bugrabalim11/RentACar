using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimDtos
{
    public class UserOperationClaimCreateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir kullanıcı seçiniz!")]
        public int UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir rütbe seçiniz!")]
        public int OperationClaimId { get; set; }
    }
}
