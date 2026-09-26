using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.AuthDtos;
using RentACar.Core.Entities.DTOs.UserDtos;
using System.Security.Claims;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // 1. SİPARİŞ: Garson formu direkt mutfağa (AuthService) iletir.
            // Hata durumunu Middleware halledeceği için garson sadece Mutlu Senaryoya (Happy Path) odaklanır!
            var userToLogin = await _authService.Login(userForLoginDto);

            // 2. BİLET KESİMİ: Şef onayladıysa VIP biletini (Token) bas.
            var result = await _authService.CreateAccessToken(userToLogin.Data);

            // 3. TESLİMAT: Bileti müşteriye teslim et. (200 OK)
            return Ok(result.Data);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // 1. SİPARİŞ: Kayıt formunu mutfağa yolla (Blender çalışsın).  
            var registerResult = await _authService.Register(userForRegisterDto, userForRegisterDto.Password);

            // 2. BİLET KESİMİ: Kayıt başarılıysa direkt Token üret.
            var result = await _authService.CreateAccessToken(registerResult.Data);

            // 3. TESLİMAT: (200 OK)
            return Ok(result.Data);
        }

        // [ApiController] aslında hepsine [FromBody] ekliyor ama biz Explicit (Açıkça belirtmek) yaptık.
        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto userForChangePasswordDto)
        {
            // 1. KİMLİK TESPİTİ: Adamın Token cüzdanına bak, 'NameIdentifier' etiketli kartı (Id) bul.
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(userIdString);

            // 2. SİPARİŞ: Şifre değiştirme talebini mutfağa ilet.
            var result = await _authService.ChangePassword(userId, userForChangePasswordDto);

            // 3. TESLİMAT: (200 OK)
            return Ok(result);
        }
    }
}