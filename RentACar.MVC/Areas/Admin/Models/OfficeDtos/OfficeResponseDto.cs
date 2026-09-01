namespace RentACar.MVC.Areas.Admin.Models.OfficeDtos
{
    public class OfficeResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<OfficeResultDto> Data { get; set; } = null!;
    }
}
