using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.OfficeDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeService _officeService;

        public OfficesController(IOfficeService officeService)
        {
            _officeService = officeService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _officeService.GetAllAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _officeService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(OfficeCreateDto officeAddDto)
        {
            var result = await _officeService.AddAsync(officeAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, OfficeUpdateDto officeUpdateDto)
        {
            if (id != officeUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen ofis ID'si eşleşmiyor!");
            }

            var result = await _officeService.UpdateAsync(officeUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _officeService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
