using RentACar.Core.Entities.Concrete;

namespace RentACar.DataAccess.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<OperationClaim>> GetClaimsAsync(User user);
    }
}
