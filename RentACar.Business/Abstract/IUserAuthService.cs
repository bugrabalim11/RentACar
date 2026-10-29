using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IUserAuthService
    {
        Task<IDataResult<int>> AddAsync(User user);
        Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user);
        Task<IDataResult<User>> GetByMailAsync(string email);
        Task<IDataResult<User>> GetByIdForAuthAsync(int id);
        Task<IResult> UpdateForAuthAsync(User user);
        Task<IResult> CheckIfEmailExistsAsync(string email);
    }
}
