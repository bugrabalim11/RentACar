namespace RentACar.MVC.Areas.Admin.Models.ContactInfoDtos
{
    public class ContactInfoResultDto
    {
        public int Id { get; set; }
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
