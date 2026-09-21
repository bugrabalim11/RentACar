namespace RentACar.Core.Entities.DTOs.UserDtos
{
    public class UserChangePasswordDto : IDto
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
