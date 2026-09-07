using RentACar.Core.Utilities.Results;
using RentACar.Dtos.RentalDtos;

namespace RentACar.Business.Abstract
{
    public interface IRentalService
    {
        Task<IDataResult<List<RentalResultDto>>> GetAllAsync();
        Task<IDataResult<RentalDetailDto>> GetByIdAsync(int id);
        Task<IDataResult<List<RentalResultDto>>> GetAllByUserIdAsync(int userId);
        Task<IDataResult<RentalDetailDto>> GetMyRentalByIdAsync(int rentalId, int userId);
        Task<IResult> CheckIfAnyRentalExistsByOfficeIdAsync(int officeId);
        Task<IResult> AddAsync(RentalCreateDto rentalAddDto, int userId);
        Task<IResult> AddByAdminAsync(RentalCreateByAdminDto rentalAddByAdminDto);
        Task<IResult> UpdateAsync(RentalUpdateDto rentalUpdateDto);
        Task<IResult> UpdateMyRentalAsync(int userId, int rentalId, RentalUpdateReturnDateDto rentalUpdateReturnDateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
