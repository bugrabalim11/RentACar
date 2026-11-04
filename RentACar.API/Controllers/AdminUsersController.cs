using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.UserDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUsersController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateByAdminAsync(int id, UserUpdateByAdminDto userUpdateForAdminDto)
        {
            if (id != userUpdateForAdminDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen kullanıcı ID'si eşleşmiyor!");
            }
            var result = await _adminUserService.UpdateForAdminAsync(userUpdateForAdminDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateByAdminAsync(UserCreateByAdminDto userCreateForAdminDto)
        {
            var result = await _adminUserService.CreateForAdminAsync(userCreateForAdminDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminUserService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> RestoreAsync(int id)
        {
            var result = await _adminUserService.RestoreAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _adminUserService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _adminUserService.GetByIdAsync(id);
            return Ok(result);
        }
    }
}
