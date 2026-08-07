using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.AuthDtos;
using RentACar.Dtos.UserDtos;

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
                return BadRequest(userToLogin);
            }

            // 2. Şef onaylarsa adamın biletini (Token) bas
            var result = await _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                // Bileti müşteriye teslim et
                return Ok(result.Data);
            }

            return BadRequest(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // 2. Şefe kayıt formunu ve şifreyi gönder (Blender çalışsın)
            var registerResult = await _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            if (!registerResult.Success)
            {
                return BadRequest(registerResult);
            }

            // 3. Kayıt başarılıysa VIP bileti bas ve teslim et
            var result = await _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        // [ApiController] aslında hepsine [FromBody] ekliyor ama biz Explicit (Açıkça belirtmek) yaptık.
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto userForChangePasswordDto)
        {
            var result = await _authService.ChangePassword(userForChangePasswordDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}