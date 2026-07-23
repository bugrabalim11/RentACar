using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;

namespace RentACar.DataAccess.Abstract
{
    public interface IUserOperationClaimRepository : IRepository<UserOperationClaim>
    {
        List<UserOperationClaimDetailDto> GetClaimDetails();
    }
}
