using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.OperationClaimDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/operationclaims")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminOperationClaimsController : ControllerBase
    {
        private readonly IAdminOperationClaimService _adminOperationClaimService;

        public AdminOperationClaimsController(IAdminOperationClaimService adminOperationClaimService)
        {
            _adminOperationClaimService = adminOperationClaimService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(OperationClaimCreateDto operationClaimAddDto)
        {
            var result = await _adminOperationClaimService.AddAsync(operationClaimAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, OperationClaimUpdateDto operationClaimUpdateDto)
        {
            if (operationClaimUpdateDto.Id != id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen yetki ID'si eşleşmiyor!");
            }

            var result = await _adminOperationClaimService.UpdateAsync(operationClaimUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminOperationClaimService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
