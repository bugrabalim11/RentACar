using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities.Concrete;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class UserOperationClaimRepository : Repository<UserOperationClaim>, IUserOperationClaimRepository
    {
        private readonly RentACarContext _context;
        public UserOperationClaimRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserOperationClaimDetailDto>> GetClaimDetailsAsync()
        {
            var result = from uoc in _context.UserOperationClaims
                         join u in _context.Users
                         on uoc.UserId equals u.Id
                         join oc in _context.OperationClaims
                         on uoc.OperationClaimId equals oc.Id
                         select new UserOperationClaimDetailDto
                         {
                             Id = uoc.Id,  
                             UserId = uoc.UserId,
                             UserFullName = u.FirstName + " " + u.LastName,   
                             OperationClaimId = oc.Id,
                             ClaimName = oc.Name  
                         };
            return await result.ToListAsync();
        }

        public async Task<List<UserOperationClaimDetailDto>> GetMyOperationClaimsAsync(int userId)
        {
            var result = from uoc in _context.UserOperationClaims
                         join u in _context.Users
                         on uoc.UserId equals u.Id                     
                         join oc in _context.OperationClaims
                         on uoc.OperationClaimId equals oc.Id
                         where uoc.UserId == userId
                         select new UserOperationClaimDetailDto
                         {
                             Id = uoc.Id,  
                             UserId = uoc.UserId,
                             UserFullName = u.FirstName + " " + u.LastName,  
                             OperationClaimId = oc.Id,
                             ClaimName = oc.Name  
                         };

            return await result.ToListAsync();
        }
    }
}
