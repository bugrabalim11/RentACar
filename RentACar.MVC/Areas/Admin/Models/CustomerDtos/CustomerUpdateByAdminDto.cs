using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.CustomerDtos
{
    public class CustomerUpdateByAdminDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir müşteri seçiniz!")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen TC kimlik numarasını boş geçmeyiniz!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC kimlik numarası tam 11 haneli olmalıdır!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "TC kimlik numarası sadece rakamlardan oluşmalıdır!")]
        public string NationalIdentity { get; set; } = null!;
        public int DrivingLicenseYear { get; set; }
    }
}
