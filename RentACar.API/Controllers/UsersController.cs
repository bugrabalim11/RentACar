using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Extensions;
using RentACar.Dtos.UserDtos;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfileAsync()
        {
            int userId = User.GetUserId();
            var result = await _userService.GetMyProfile(userId);
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateMyProfile(UserProfileUpdateDto userProfileUpdateDto)
        {
            int userId = User.GetUserId();
            var result = await _userService.UpdateMyProfileAsync(userId, userProfileUpdateDto);
            return Ok(result);
        }

        [HttpDelete("profile")]
        public async Task<IActionResult> DeleteMyAccount()
        {
            int userId = User.GetUserId();
            var result = await _userService.DeleteMyAccountAsync(userId);
            return Ok(result);
        }
    }
}
