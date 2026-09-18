using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.CustomerDtos
{
    public class CustomerCreateByAdminDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir kullanıcı seçiniz!")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Lütfen TC kimlik numarasını boş geçmeyiniz!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC kimlik numarası tam 11 haneli olmalıdır!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "TC kimlik numarası sadece rakamlardan oluşmalıdır!")]
        public string NationalIdentity { get; set; } = null!;

        // Data Annotations'ta DateTime.Now.Year gibi dinamik kodlar doğrudan yazılamaz
        public int DrivingLicenseYear { get; set; }
    }
}
