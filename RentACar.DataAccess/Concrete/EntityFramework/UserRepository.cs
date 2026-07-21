using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities.Concrete;
using RentACar.DataAccess.Abstract;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    // Genel yetenekleri Repository'den alıyor, özel kuralları IUserRepository'den uyguluyor
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly RentACarContext _context;

        // Tesisatçı (Constructor) kabloyu takıyor ve base (ana depo) sınıfa iletiyor
        public UserRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        // Özel görevimiz: 3 Tabloyu (Claims, UserClaims, User) birbirine dikmek!
        public Task<List<OperationClaim>> GetClaimsAsync(User user)
        {
            // LINQ ile SQL Join İşlemi
            var result = from operationClaim in _context.OperationClaims
                         join userOperationClaim in _context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                         where userOperationClaim.UserId == user.Id
                         select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };

            return result.ToListAsync();
        }
    }
}
