using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities.Concrete;
using RentACar.DataAccess.Abstract;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly RentACarContext _context;

        public UserRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<OperationClaim>> GetClaimsAsync(User user)
        {
            var result = from operationClaim in _context.OperationClaims
                         join userOperationClaim in _context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                         where userOperationClaim.UserId == user.Id
                         select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };

            return result.ToListAsync();
        }
    }
}
