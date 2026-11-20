using RentACar.MAUI.Models;
using RentACar.MAUI.Services.Abstract;
using System.Net.Http.Json;

namespace RentACar.MAUI.Services.Concrete
{
    public class CarService : ICarService
    {
        private readonly HttpClient _httpClient;

        public CarService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CarDto>> GetAllCarsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CarDto>>>("api/cars");
            return response?.Data ?? new List<CarDto>();
        }
    }
}
