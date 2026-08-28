namespace RentACar.MVC.Areas.Admin.Models.BrandDtos
{
    public class BrandResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<ResultBrandDto> Data { get; set; } = null!;
    }
}
