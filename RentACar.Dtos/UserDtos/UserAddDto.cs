using RentACar.Core.Entities;

namespace RentACar.Dtos.UserDtos
{
    public class UserAddDto : IDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
