using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    /// <summary>
    /// Bu sözleşme, AuthManager (Business) ile UserRepository (Data) arasındaki İÇ HAT (Internal) operasyonlarını yönetir.
    /// Dış dünyaya (Controller/API) açılmadığı için DTO yerine kasıtlı olarak saf 'User' Entity'si kullanılmıştır.
    /// </summary>
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
