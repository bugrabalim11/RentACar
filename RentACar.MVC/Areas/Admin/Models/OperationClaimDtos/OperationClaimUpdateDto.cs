using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.OperationClaimDtos
{
    public class OperationClaimUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen statüyü boş geçemeyiniz!")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Statü en az 2, en fazla 10 karakter olamalıdır!")]
        public string Name { get; set; } = null!;
    }
}
