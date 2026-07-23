using RentACar.Core.Entities.Concrete;
using RentACar.DataAccess.Abstract;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class UserOperationClaimRepository : Repository<UserOperationClaim>, IUserOperationClaimRepository
    {
        public UserOperationClaimRepository(RentACarContext context) : base(context)
        {
        }
    }
}
