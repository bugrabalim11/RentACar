using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.DTOs.OperationClaimDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimsController : ControllerBase
    {
        private readonly IOperationClaimService _operationClaimService;

        public OperationClaimsController(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _operationClaimService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _operationClaimService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddAsync(OperationClaimAddDto operationClaimAddDto)
        {
            var result = await _operationClaimService.AddAsync(operationClaimAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(OperationClaimUpdateDto operationClaimUpdateDto)
        {
            var result = await _operationClaimService.UpdateAsync(operationClaimUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _operationClaimService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
