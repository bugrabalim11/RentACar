using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.CarImageDtos
{
    public class CarImageCreateDto
    {
        public int CarId { get; set; }
        [Required(ErrorMessage = "Lütfen bir resim seçiniz!")]
        public IFormFile ImageFile { get; set; } = null!;
    }
}
