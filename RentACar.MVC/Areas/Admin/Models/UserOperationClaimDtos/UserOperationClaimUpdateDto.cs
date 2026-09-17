using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.UserOperationClaimDtos
{
    public class UserOperationClaimUpdateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Kayıt kimliği geçersiz!")]
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir rütbe seçiniz!")]
        public int OperationClaimId { get; set; }
        public int UserId { get; set; }
    }
}
