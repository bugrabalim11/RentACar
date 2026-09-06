namespace RentACar.MVC.Areas.Admin.Models.CarImageDtos
{
    public class CarImageCreateDto
    {
        public int CarId { get; set; }
        public IFormFile ImageFile { get; set; } = null!;
    }
}
