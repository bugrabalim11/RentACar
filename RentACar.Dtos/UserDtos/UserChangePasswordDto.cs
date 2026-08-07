using RentACar.Core.Entities;

namespace RentACar.Dtos.UserDtos
{
    public class UserChangePasswordDto : IDto
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
