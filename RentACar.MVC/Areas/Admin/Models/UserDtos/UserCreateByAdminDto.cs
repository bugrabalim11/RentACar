using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.UserDtos
{
    public class UserCreateByAdminDto
    {
        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz!")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen şifrenizi giriniz!")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen kullanıcı adını boş geçemeyiniz!")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Kullanıcı adı en az 2, en fazla 50 karakter olamalıdır!")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen kullanıcı soyadını boş geçemeyiniz!")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Kullanıcı soyadı en az 2, en fazla 50 karakter olamalıdır!")]
        public string LastName { get; set; } = null!;

        public int? OperationClaimId { get; set; }
    }
}
