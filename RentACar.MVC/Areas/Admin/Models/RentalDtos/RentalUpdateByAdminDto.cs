using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.RentalDtos
{
    public class RentalUpdateByAdminDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir kiralama seçiniz!")]
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir araç seçiniz!")]
        public int CarId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir ofis seçiniz!")]
        public int PickUpOfficeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir ofis seçiniz!")]
        public int DropOffOfficeId { get; set; }

        [Required(ErrorMessage = "Lütfen araç kiralama tarihini boş geçmeyiniz!")]
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
