using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface ICarReferenceService
    {
        Task<IResult> CheckIfBrandIsUsedByAnyCarAsync(int brandId);
        Task<IResult> CheckIfColorIsUsedByAnyCarAsync(int colorId);
    }
}
