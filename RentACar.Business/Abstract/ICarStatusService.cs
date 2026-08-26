using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Abstract
{
    public interface ICarStatusService
    {
        Task<IResult> CheckIfCarIsRentedAsync(int carId, DateTime startDate, DateTime? endDate);
        Task<IResult> CheckIfCarIsInMaintenanceAsync(int carId, DateTime startDate, DateTime? endDate);
    }
}
