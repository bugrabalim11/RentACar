using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.UserOperationClaimDtos
{
    public class UserOperationClaimCreateDto : IDto
    {
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
    }
}
