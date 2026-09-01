namespace RentACar.MVC.Areas.Admin.Models.OfficeDtos
{
    public class GetByIdOfficeResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public OfficeUpdateDto Data { get; set; } = null!;
    }
}
