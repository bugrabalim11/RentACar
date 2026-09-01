namespace RentACar.MVC.Areas.Admin.Models.ContactInfoDtos
{
    public class GetByIdContactInfoResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public ContactInfoUpdateDto Data { get; set; } = null!;
    }
}
