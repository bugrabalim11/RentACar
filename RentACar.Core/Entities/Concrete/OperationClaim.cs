namespace RentACar.Core.Entities.Concrete
{
    public class OperationClaim : BaseEntity, IEntity
    {
        // Id, CreatedDate, UpdatedDate, DeletedDate ve IsDeleted otomatik olarak BaseEntity'den (Babadan) gelir!
        public string Name { get; set; } = null!;
    }
}
