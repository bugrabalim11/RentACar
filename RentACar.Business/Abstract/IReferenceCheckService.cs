using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IReferenceCheckService
    {
        Task<IResult>CheckIfBrandExistsAsync(int brandId);
        Task<IResult>CheckIfColorExistsAsync(int colorId);
        Task<IResult> CheckIfColorIsUsedByAnyCarAsync(int colorId);
        Task<IResult> CheckIfUserExistsAsync(int userId);
    }
}
