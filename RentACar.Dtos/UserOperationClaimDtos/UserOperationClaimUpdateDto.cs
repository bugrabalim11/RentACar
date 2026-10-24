using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.UserOperationClaimDtos
{
    public class UserOperationClaimUpdateDto : IDto
    {
        public int Id { get; set; }
        public int OperationClaimId { get; set; }
    }
}
