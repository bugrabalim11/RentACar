using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.AuthDtos;

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
            // 1. Şefe formu ver
            var userToLogin = await _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            // 2. Şef onaylarsa adamın biletini (Token) bas
            var result = await _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                // Bileti müşteriye teslim et
                return Ok(result.Data);
            }

            return BadRequest(result.Data);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // 1. Kapıdaki kontrol: Bu e-posta bizde var mı?
            var userExist = await _authService.UserExist(userForRegisterDto.Email);
            if (!userExist.Success)
            {
                // "Kullanıcı zaten mevcut" hatası
                return BadRequest(userExist.Message);
            }

            // 2. Şefe kayıt formunu ve şifreyi gönder (Blender çalışsın)
            var registerResult = await _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            if (!registerResult.Success)
            {
                return BadRequest(registerResult.Message);
            }

            // 3. Kayıt başarılıysa VIP bileti bas ve teslim et
            var result = await _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Data);
        }
    }
}