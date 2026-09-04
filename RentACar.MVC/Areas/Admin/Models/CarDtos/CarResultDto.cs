namespace RentACar.MVC.Areas.Admin.Models.CarDtos
{
    public class CarResultDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public int Kilometer { get; set; }
        public decimal DailyPrice { get; set; }
    }
}
