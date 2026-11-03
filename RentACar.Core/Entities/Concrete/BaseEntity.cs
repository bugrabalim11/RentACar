namespace RentACar.Core.Entities.Concrete
{
    /// <summary>
    /// Veritabanındaki tüm tabloların (Entity) türediği temel kalıtım (Base) sınıfıdır.
    /// Ortak alanları (Id, Audit Log tarihleri ve Soft Delete durumu) merkezi olarak yönetir.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

        public BaseEntity()
        {
            CreatedDate = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}