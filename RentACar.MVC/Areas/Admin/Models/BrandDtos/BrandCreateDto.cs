using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.BrandDtos
{
    public class BrandCreateDto
    {
        [Required(ErrorMessage = "Lütfen marka adını boş bırakmayınız!")]
        [MinLength(2, ErrorMessage = "Araç markası en az 2 karakter olmalıdır!")]
        [MaxLength(30, ErrorMessage = "Araç markası en fazla 30 karakter olmalıdır!")]
        public string Name { get; set; } = null!;
    }
}
