using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.AuthDtos
{
    public class UserForLoginDto
    {
        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        public string Password { get; set; } = null!;
    }
}
