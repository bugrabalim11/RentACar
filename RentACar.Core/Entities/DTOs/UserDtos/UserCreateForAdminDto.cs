namespace RentACar.Core.Entities.DTOs.UserDtos
{
    public class UserCreateForAdminDto : IDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int OperationClaimId { get; set; }
    }
}
