namespace RentACar.MVC.Areas.Admin.Models.BrandDtos
{
    public class GetByIdBrandResponseDto
    {
        // Dış kolinin üzerindeki yazılar
        public bool Success { get; set; }
        public string Message { get; set; } = null!;

        // Matruşkanın içindeki asıl değer! 
        // Tipini UpdateBrandDto veriyoruz ki, formumuzun beklediği o 'Id' ve 'Name' yapısıyla tam eşleşsin.
        public BrandUpdateDto Data { get; set; } = null!;
    }
}
