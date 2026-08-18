using RentACar.Core.Utilities.Results;
using RentACar.Dtos.RentalDtos;

namespace RentACar.Business.Abstract
{
    public interface IRentalService
    {
        Task<IDataResult<List<RentalListDto>>> GetAllAsync();
        Task<IDataResult<RentalListDto>> GetByIdAsync(int id);
        Task<IDataResult<List<RentalListDto>>> GetAllByUserIdAsync(int userId);
        Task<IDataResult<RentalListDto>> GetMyRentalByIdAsync(int rentalId, int userId);
        Task<IResult> CheckIfAnyRentalExistsByOfficeIdAsync(int officeId);
        Task<IResult> AddAsync(RentalAddDto rentalAddDto, int userId);
        Task<IResult> UpdateAsync(RentalUpdateDto rentalUpdateDto);
        Task<IResult> UpdateMyRentalAsync(int userId, int rentalId, RentalUpdateReturnDateDto rentalUpdateReturnDateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
