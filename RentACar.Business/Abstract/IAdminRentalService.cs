using RentACar.Core.Utilities.Results;
using RentACar.Dtos.RentalDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminRentalService
    {
        Task<IDataResult<List<RentalResultDto>>> GetAllAsync();
        Task<IDataResult<RentalDetailDto>> GetByIdAsync(int id);
        Task<IDataResult<int>> AddByAdminAsync(RentalCreateByAdminDto rentalAddByAdminDto);
        Task<IResult> UpdateByAdminAsync(RentalUpdateByAdminDto rentalUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
