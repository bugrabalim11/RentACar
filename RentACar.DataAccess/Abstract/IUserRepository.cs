using RentACar.Core.Entities.Concrete;

namespace RentACar.DataAccess.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        // Standart Ekle/Sil/Güncelle zaten IEntityRepository'den gelecek.
        // Bu bizim özel görevimiz: Kullanıcının rollerini getirmek!
        Task<List<OperationClaim>> GetClaimsAsync(User user);
    }
}
