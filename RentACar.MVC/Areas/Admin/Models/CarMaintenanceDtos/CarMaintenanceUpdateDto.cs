using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.CarMaintenanceDtos
{
    public class CarMaintenanceUpdateDto
    {
        [Required(ErrorMessage = "Lütfen geçerli bir araç bakımı seçiniz!")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen açıklamayı doldurunuz!")]
        [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olmalıdır!")]
        [MinLength(10, ErrorMessage = "Açıklama en az 10 karakter olmalıdır!")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen giriş tarihini doldurunuz!")]
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
