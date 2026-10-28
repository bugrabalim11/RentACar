using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Extensions;
using RentACar.Dtos.UserDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/users")]
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

        [HttpGet("admin-details")]
        public async Task<IActionResult> GetAllForAdminAsync()
        {
            var result = await _adminUserService.GetAllForAdminAsync();
            return Ok(result);
        }

        [HttpGet("{id}/update-form")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            var result = await _adminUserService.GetByIdForUpdateAsync(id);
            return Ok(result);
        }
    }
}
