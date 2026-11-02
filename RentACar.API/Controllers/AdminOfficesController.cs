using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.OfficeDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/offices")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminOfficesController : ControllerBase
    {
        private readonly IAdminOfficeService _adminOfficeService;

        public AdminOfficesController(IAdminOfficeService adminOfficeService)
        {
            _adminOfficeService = adminOfficeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(OfficeCreateDto officeAddDto)
        {
            var result = await _adminOfficeService.AddAsync(officeAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, OfficeUpdateDto officeUpdateDto)
        {
            if (id != officeUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen ofis ID'si eşleşmiyor!");
            }

            var result = await _adminOfficeService.UpdateAsync(officeUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminOfficeService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
