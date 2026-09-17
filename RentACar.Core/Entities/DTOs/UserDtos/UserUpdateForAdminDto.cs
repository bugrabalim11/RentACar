namespace RentACar.Core.Entities.DTOs.UserDtos
{
    public class UserUpdateForAdminDto : IDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? OperationClaimId { get; set; }
    }
}
