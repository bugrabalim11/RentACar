namespace RentACar.MVC.Areas.Admin.Models.CarImageDtos
{
    public class CarImageResultDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarName { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public DateTime UploadDate { get; set; }
    }
}
