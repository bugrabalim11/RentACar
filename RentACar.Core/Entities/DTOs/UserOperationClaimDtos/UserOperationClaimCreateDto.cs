namespace RentACar.Core.Entities.DTOs.UserOperationClaimDtos
{
    public class UserOperationClaimCreateDto : IDto
    {
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
    }
}
