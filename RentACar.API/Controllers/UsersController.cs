using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.UserDtos;
using RentACar.Core.Exceptions;
using System.Security.Claims;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserOperationClaimService _userOperationClaimService;

        public UsersController(IUserService userService, IUserOperationClaimService userOperationClaimService)
        {
            _userService = userService;
            _userOperationClaimService = userOperationClaimService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getallforadmin")]
        public async Task<IActionResult> GetAllForAdminAsync()
        {
            var result = await _userService.GetAllForAdminAsync();
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getbyidforupdate/{id}")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            var result = await _userService.GetByIdForUpdateAsync(id);
            return Ok(result);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfileAsync()
        {
            int userId = GetUserIdFromClaims();
            var result = await _userService.GetMyProfile(userId);
            return Ok(result);
        }

        [HttpGet("my-claims")]
        public async Task<IActionResult> GetMyOperationClaims()
        {
            int userId = GetUserIdFromClaims();
            var result = await _userOperationClaimService.GetMyOperationClaimsAsync(userId);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("updateforadmin/{id}")]
        public async Task<IActionResult> UpdateByAdminAsync(int id, UserUpdateByAdminDto userUpdateForAdminDto)
        {
            if (id != userUpdateForAdminDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen kullanıcı ID'si eşleşmiyor!");
            }
            var result = await _userService.UpdateForAdminAsync(userUpdateForAdminDto);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("createforadmin")]
        public async Task<IActionResult> CreateByAdminAsync(UserCreateByAdminDto userCreateForAdminDto)
        {
            var result = await _userService.CreateForAdminAsync(userCreateForAdminDto);
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateMyProfile(UserProfileUpdateDto userProfileUpdateDto)
        {
            int userId = GetUserIdFromClaims();
            var result = await _userService.UpdateMyProfileAsync(userId, userProfileUpdateDto);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userService.DeleteAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("restore/{id}")]
        public async Task<IActionResult> RestoreAsync(int id)
        {
            var result = await _userService.RestoreAsync(id);
            return Ok(result);
        }

        [HttpDelete("profile")]
        public async Task<IActionResult> DeleteMyAccount()
        {
            int userId = GetUserIdFromClaims();
            var result = await _userService.DeleteAsync(userId);
            return Ok(result);
        }

        private int GetUserIdFromClaims()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new BusinessException("Kimlik doğrulama hatası! Geçerli bir token bulunamadı!");
            }
            return Convert.ToInt32(userIdString);
        }
    }
}
