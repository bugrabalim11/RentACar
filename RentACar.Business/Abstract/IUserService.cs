using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Results;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<UserListDto>>> GetAllAsync();
        Task<IDataResult<UserListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(UserAddDto userAddDto);
        Task<IResult> UpdateAsync(UserUpdateDto userUpdateDto);
        Task<IResult> DeleteAsync(int id);

        // Dikkat: DTO değil, doğrudan varlığın (Entity) kendisini dönüyoruz!
        Task<IDataResult<User>> GetByMailAsync(string email);
        Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user);
    }
}
