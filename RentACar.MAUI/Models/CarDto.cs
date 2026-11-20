namespace RentACar.MAUI.Models
{
    public class CarDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public int Kilometer { get; set; }
        public decimal DailyPrice { get; set; }
        public string Plate { get; set; } = null!;
        public string CoverImageUrl { get; set; } = null!;
    }
}
