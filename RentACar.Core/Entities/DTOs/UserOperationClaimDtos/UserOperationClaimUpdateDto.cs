namespace RentACar.Core.Entities.DTOs.UserOperationClaimDtos
{
    public class UserOperationClaimUpdateDto : IDto
    {
        public int Id { get; set; }
        public int OperationClaimId { get; set; }
    }
}
