namespace RentACar.MVC.Areas.Admin.Models.ContactMessageDtos
{
    public class ContactMessageResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<ContactMessageResultDto> Data { get; set; } = null!;
    }
}
