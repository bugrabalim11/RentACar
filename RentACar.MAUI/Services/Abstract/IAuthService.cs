using RentACar.MAUI.Models;

namespace RentACar.MAUI.Services.Abstract
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
