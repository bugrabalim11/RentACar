namespace RentACar.MVC.Areas.Admin.Models.CarImageDtos
{
    public class CarImageDeletedDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = null!;
        public int CarId { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
