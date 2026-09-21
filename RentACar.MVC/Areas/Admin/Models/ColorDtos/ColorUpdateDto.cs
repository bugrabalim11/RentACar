using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.ColorDtos
{
    public class ColorUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen renk adını boş bırakmayınız!")]
        [MinLength(2, ErrorMessage = "Renk adı en az 2 karakter olmalıdır!")]
        [MaxLength(20, ErrorMessage = "Renk adı en fazla 20 karakter olmalıdır!")]
        public string Name { get; set; } = null!;
    }
}
