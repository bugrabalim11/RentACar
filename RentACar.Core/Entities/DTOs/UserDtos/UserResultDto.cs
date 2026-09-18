namespace RentACar.Core.Entities.DTOs.UserDtos
{
    public class UserResultDto : IDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        // CSHTML de sadece okuma için ekledik
        public string UserDisplayInfo => $"{FirstName} {LastName} - {Email}";
    }
}
