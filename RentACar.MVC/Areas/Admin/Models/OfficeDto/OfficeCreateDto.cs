using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.OfficeDto
{
    public class OfficeCreateDto
    {
        [Required(ErrorMessage = "Lütfen ofis adını boş bırakmayın!")]
        [MinLength(2, ErrorMessage = "Ofis adı en az 2 karakter olmalıdır!")]
        [MaxLength(30, ErrorMessage = "Ofis adı en fazla 30 karakter olmalıdır!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen ofis adını boş bırakmayın!")]
        [MinLength(2, ErrorMessage = "Ofis adı en az 2 karakter olmalıdır!")]
        [MaxLength(30, ErrorMessage = "Ofis adı en fazla 30 karakter olmalıdır!")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen iletişim numarasını boş bırakmayın!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "İletişim numarası en fazla 11 karakter olmalıdır!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "İletişim numarası sadece rakamlardan oluşmalıdır!")]
        public string ContactNumber { get; set; } = null!;
    }
}
