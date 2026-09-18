namespace RentACar.MVC.Areas.Admin.Models.CustomerDtos
{
    public class CustomerResultDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string NationalIdentity { get; set; } = null!;

        // TODO İlerde bunu mapping ile yapıcaz
        public string FullName => $"{FirstName} {LastName}";
    }
}
