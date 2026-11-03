using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.UserOperationClaimDtos
{
    public class UserOperationClaimDetailDto : IDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = null!;  
        public int OperationClaimId { get; set; }
        public string ClaimName { get; set; } = null!;  
    }
}
