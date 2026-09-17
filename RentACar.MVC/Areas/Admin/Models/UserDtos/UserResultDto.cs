namespace RentACar.MVC.Areas.Admin.Models.UserDtos
{
    public class UserResultDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        // TODO TEK BİR FULLNAME PROPU YAZ MANUEL MAPPİNG YAP CREATE BİTTİKEN SONRA
        // CSHTML de sadece okuma için ekledik
        public string FullName => $"{FirstName} {LastName}";
    }
}
