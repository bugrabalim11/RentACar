namespace RentACar.MVC.Areas.Admin.Models.CustomerDtos
{
    public class CustomerResultDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string NationalIdentity { get; set; } = null!;

        // Not: Dropdown'da kullanıcı dostu (UX) görünüm için computed property kullanıldı.
        public string FullName => $"{FirstName} {LastName} - {NationalIdentity}";
    }
}
