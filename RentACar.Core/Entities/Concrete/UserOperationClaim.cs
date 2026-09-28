namespace RentACar.Core.Entities.Concrete
{
    public class UserOperationClaim : BaseEntity, IEntity
    {
        public int UserId { get; set; }
        public int? OperationClaimId { get; set; }
    }
}
