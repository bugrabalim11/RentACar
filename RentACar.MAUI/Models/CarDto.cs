using System.Text.Json.Serialization;

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

        // TODO: Canlıya alırken gerçek HTTPS domain adresine çekilecek ve gerçek domain geleceği için bu Replace kaldırılacak
        public string FullImageUrl => string.IsNullOrWhiteSpace(CoverImageUrl)
            ? string.Empty
            : CoverImageUrl.Replace("https://localhost:7085", "http://10.0.2.2:5210");
    }
}
