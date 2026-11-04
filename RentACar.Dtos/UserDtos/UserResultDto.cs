using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.UserDtos
{
    public class UserResultDto : IDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int OperationClaimId { get; set; }
        public string UserDisplayInfo => $"{FirstName} {LastName} - {Email}";
    }
}
