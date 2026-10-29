using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface IReferenceCheckService
    {
        Task<IResult> CheckIfOfficeHasRentalsAsync(int officeId);
    }
}
