using RentACar.Core.Entities.Concrete;
using RentACar.DataAccess.Abstract;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class OperationClaimRepository : Repository<OperationClaim>, IOperationClaimRepository
    {
        public OperationClaimRepository(RentACarContext context) : base(context)
        {
        }
    }
}
