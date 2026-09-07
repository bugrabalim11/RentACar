using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.CarMaintenanceDtos
{
    public class CarMaintenanceCreateDto
    {
        [Required(ErrorMessage = "Lütfen geçerli bir araç seçiniz!")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Lütfen açıklamayı dolddurunuz!")]
        [MaxLength(500, ErrorMessage = "Açıklma en fazla 500 karakter olmalıdır!")]
        [MinLength(10, ErrorMessage = "Açıklma en az 10 karakter olmalıdır!")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen giriş tarihini doldurunuz!")]
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
