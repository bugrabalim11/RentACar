using RentACar.MAUI.Models;
using RentACar.MAUI.Services.Abstract;
using System.Text;
using System.Text.Json;

namespace RentACar.MAUI.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var jsonData = JsonSerializer.Serialize(loginRequestDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auths/login", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<TokenResponseDto>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return tokenData;
            }

            return null;
        }
    }
}
