using RentACar.MAUI.Models;

namespace RentACar.MAUI.Services.Abstract
{
    public interface ICarService
    {
        Task<List<CarResultDto>>GetAllCarsAsync();
    }
}
