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

        public async Task<List<CarResultDto>> GetAllCarsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CarResultDto>>>("api/cars");
            return response?.Data ?? new List<CarResultDto>();
        }

        public async Task<ApiResponse<CarDetailDto>> GetCarDetailAsync(int carId)
        {
            if (carId <= 0)
            {
                return new ApiResponse<CarDetailDto> { Success = false, Message = "Geçersiz araç bilgisi." };
            }

            var response = await _httpClient.GetAsync($"api/cars/{carId}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new ApiResponse<CarDetailDto> { Success = false, Message = string.IsNullOrWhiteSpace(errorMessage) ? "Sunucu beklenmeyen bir hata döndü." : errorMessage };
            }
            var responseJson = await response.Content.ReadFromJsonAsync<ApiResponse<CarDetailDto>>();
            if (responseJson == null)
            {
                return new ApiResponse<CarDetailDto> { Success = false, Message = "Sunucudan veri çekilemedi." };
            }
            return responseJson;
        }
    }
}