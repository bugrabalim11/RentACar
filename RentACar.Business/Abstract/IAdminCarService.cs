using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarDtos;

namespace RentACar.Business.Abstract
{
    public interface IAdminCarService
    {
        Task<IDataResult<int>> AddAsync(CarCreateDto carAddDto);
        Task<IResult> UpdateAsync(CarUpdateDto carUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
