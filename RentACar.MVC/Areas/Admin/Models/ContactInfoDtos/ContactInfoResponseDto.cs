namespace RentACar.MVC.Areas.Admin.Models.ContactInfoDtos
{
    public class ContactInfoResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<ContactInfoResultDto> Data { get; set; } = null!;
    }
}
