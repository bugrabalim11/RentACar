using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;
using RentACar.Core.Exceptions;

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
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _userOperationClaimService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserOperationClaimCreateDto userOperationClaimAddDto)
        {
            var result = await _userOperationClaimService.AddAsync(userOperationClaimAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UserOperationClaimUpdateDto userOperationClaimUpdateDto)
        {
            if (id != userOperationClaimUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen Müşteri ID'si eşleşmiyor!");
            }

            var result = await _userOperationClaimService.UpdateAsync(userOperationClaimUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userOperationClaimService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetClaimDetailsAsync()
        {
            var result = await _userOperationClaimService.GetClaimDetailsAsync();
            return Ok(result);
        }
    }
}
