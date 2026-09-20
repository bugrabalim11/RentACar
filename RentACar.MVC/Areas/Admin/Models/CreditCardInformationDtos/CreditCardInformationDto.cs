using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.CreditCardInformationDtos
{
    public class CreditCardInformationDto
    {
        [Required(ErrorMessage = "Lütfen kart sahibinin adını boş geçmeyiniz!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kart sahibinin adı ve soyadı en az 3 karakter olmalıdır!")]
        public string CardHolderFullName { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen kart numarasını boş geçmeyiniz!")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Kart numarası 16 haneli olmalıdır!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Kart numarası sadece rakamlardan oluşmalıdır!")]
        public string CardNumber { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen kart geçerlilik yılını boş geçmeyiniz!")]
        public int ExpireYear { get; set; }

        [Required(ErrorMessage = "Lütfen kart geçerlilik ayını boş geçmeyiniz!")]
        [Range(1, 12, ErrorMessage = "Ay bilgisi 1 ile 12 arasında olmalıdır!")]
        public int ExpireMonth { get; set; }

        [Required(ErrorMessage = "Lütfen CVV'yi boş geçmeyiniz!")]
        [StringLength(3,MinimumLength =3,ErrorMessage ="CVV 3 haneli olmak zorundadır!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CVV sadece rakamlardan oluşmalıdır!")]
        public string Cvv { get; set; } = null!;
    }
}
