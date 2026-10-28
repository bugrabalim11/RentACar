using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/useroperationclaims")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminUserOperationClaimsController : ControllerBase
    {
        private readonly IAdminUserOperationClaimService _adminUserOperationClaimService;

        public AdminUserOperationClaimsController(IAdminUserOperationClaimService adminUserOperationClaimService)
        {
            _adminUserOperationClaimService = adminUserOperationClaimService;
        }
     
        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserOperationClaimCreateDto userOperationClaimAddDto)
        {
            var result = await _adminUserOperationClaimService.AddAsync(userOperationClaimAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UserOperationClaimUpdateDto userOperationClaimUpdateDto)
        {
            if (id != userOperationClaimUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen yetki atama ID'si eşleşmiyor!");
            }

            var result = await _adminUserOperationClaimService.UpdateAsync(userOperationClaimUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminUserOperationClaimService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
