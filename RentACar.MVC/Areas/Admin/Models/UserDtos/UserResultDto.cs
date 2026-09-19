namespace RentACar.MVC.Areas.Admin.Models.UserDtos
{
    public class UserResultDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        // Not: Dropdown'da kullanıcı dostu (UX) görünüm için computed property kullanıldı.
        public string UserDisplayInfo => $"{FirstName} {LastName} - {Email}";
    }
}
