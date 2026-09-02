namespace RentACar.MVC.Areas.Admin.Models.ContactMessageDtos
{
    public class GetByIdContactMessageResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public ContactMessageResultDto Data { get; set; } = null!;
    }
}
