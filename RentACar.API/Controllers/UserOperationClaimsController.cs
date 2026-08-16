using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UserOperationClaimsController : ControllerBase
    {
        private readonly IUserOperationClaimService _userOperationClaimService;

        public UserOperationClaimsController(IUserOperationClaimService userOperationClaimService)
        {
            _userOperationClaimService = userOperationClaimService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await _userOperationClaimService.GetAllAsync();
            if (results.Success)
            {
                return Ok(results);
            }
            return BadRequest(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _userOperationClaimService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UserOperationClaimAddDto userOperationClaimAddDto)
        {
            var result = await _userOperationClaimService.AddAsync(userOperationClaimAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UserOperationClaimUpdateDto userOperationClaimUpdateDto)
        {
            if (id != userOperationClaimUpdateDto.Id) return BadRequest("URL'deki ID ile gönderilen Müşteri ID'si eşleşmiyor!");

            var result = await _userOperationClaimService.UpdateAsync(userOperationClaimUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userOperationClaimService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetClaimDetailsAsync()
        {
            var result = await _userOperationClaimService.GetClaimDetailsAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
