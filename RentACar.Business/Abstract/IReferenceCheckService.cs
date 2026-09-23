using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IReferenceCheckService
    {
        Task<IResult>CheckIfBrandExistsAsync(int brandId);
        Task<IResult>CheckIfColorExistsAsync(int colorId);
    }
}
