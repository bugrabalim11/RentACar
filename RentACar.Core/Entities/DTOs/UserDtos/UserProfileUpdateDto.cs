namespace RentACar.Core.Entities.DTOs.UserDtos
{
    public class UserProfileUpdateDto : IDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
