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
        Task<IDataResult<int>> AddAsync(RentalCreateDto rentalAddDto, int userId);
        Task<IDataResult<int>> AddByAdminAsync(RentalCreateByAdminDto rentalAddByAdminDto);
        Task<IResult> UpdateByAdminAsync(RentalUpdateByAdminDto rentalUpdateDto);
        Task<IResult> UpdateMyRentalAsync(int userId, int rentalId, RentalUpdateReturnDateDto rentalUpdateReturnDateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
