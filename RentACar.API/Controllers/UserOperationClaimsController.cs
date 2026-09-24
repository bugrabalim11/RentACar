using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;
using RentACar.Core.Exceptions;
using System.Security.Claims;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserOperationClaimsController : ControllerBase
    {
        private readonly IUserOperationClaimService _userOperationClaimService;

        public UserOperationClaimsController(IUserOperationClaimService userOperationClaimService)
        {
            _userOperationClaimService = userOperationClaimService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await _userOperationClaimService.GetAllAsync();
            return Ok(results);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _userOperationClaimService.GetByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserOperationClaimCreateDto userOperationClaimAddDto)
        {
            var result = await _userOperationClaimService.AddAsync(userOperationClaimAddDto);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UserOperationClaimUpdateDto userOperationClaimUpdateDto)
        {
            if (id != userOperationClaimUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen yetki atama ID'si eşleşmiyor!");
            }

            var result = await _userOperationClaimService.UpdateAsync(userOperationClaimUpdateDto);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userOperationClaimService.DeleteAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("details")]
        public async Task<IActionResult> GetClaimDetailsAsync()
        {
            var result = await _userOperationClaimService.GetClaimDetailsAsync();
            return Ok(result);
        }

        [HttpGet("my-claims")]
        public async Task<IActionResult> GetMyClaimsAsync()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) { throw new BusinessException("Kimlik doğrulama hatası! Geçerli bir token bulunamadı!"); }
            int userId = Convert.ToInt32(userIdString);

            var result = await _userOperationClaimService.GetMyOperationClaimsAsync(userId);
            return Ok(result);
        }
    }
}