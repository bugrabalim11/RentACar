using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.UserDtos;
using RentACar.Core.Exceptions;
using RentACar.Core.Extensions;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin-details")]
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
        [HttpGet("{id}/update-form")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            var result = await _userService.GetByIdForUpdateAsync(id);
            return Ok(result);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfileAsync()
        {
            int userId = User.GetUserId();
            var result = await _userService.GetMyProfile(userId);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
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
        [HttpPost]
        public async Task<IActionResult> CreateByAdminAsync(UserCreateByAdminDto userCreateForAdminDto)
        {
            var result = await _userService.CreateForAdminAsync(userCreateForAdminDto);
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateMyProfile(UserProfileUpdateDto userProfileUpdateDto)
        {
            int userId = User.GetUserId();
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
        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> RestoreAsync(int id)
        {
            var result = await _userService.RestoreAsync(id);
            return Ok(result);
        }

        [HttpDelete("profile")]
        public async Task<IActionResult> DeleteMyAccount()
        {
            int userId = User.GetUserId();
            var result = await _userService.DeleteAsync(userId);
            return Ok(result);
        }
    }
}
