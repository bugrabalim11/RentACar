using RentACar.MAUI.Models;

namespace RentACar.MAUI.Services.Abstract
{
    public interface ICarService
    {
        // TODO: cardetail branchinden sonra dçnüş tipine ApiResponse ekle
        Task<List<CarResultDto>> GetAllCarsAsync();
        Task<ApiResponse<CarDetailDto>> GetCarDetailAsync(int carId);
    }
}
