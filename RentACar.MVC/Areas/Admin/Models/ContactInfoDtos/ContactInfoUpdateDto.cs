using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.ContactInfoDtos
{
    public class ContactInfoUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen adresi boş bırakmayın!")]
        [MinLength(10, ErrorMessage = "Adres en az 10 karakter olmalıdır!")]
        [MaxLength(100, ErrorMessage = "Adres en fazla 100 karakter olmalıdır!")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen iletişim numarasını boş bırakmayın!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "İletişim numarası en fazla 11 karakter olmalıdır!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "İletişim numarası sadece rakamlardan oluşmalıdır!")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen e-posta adresini boş bırakmayın!")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; } = null!;
    }
}
