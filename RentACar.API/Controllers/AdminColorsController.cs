using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.ColorDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/colors")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminColorsController : ControllerBase
    {
        private readonly IAdminColorService _adminColorService;

        public AdminColorsController(IAdminColorService adminColorService)
        {
            _adminColorService = adminColorService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ColorCreateDto colorAddDto)
        {
            var result = await _adminColorService.AddAsync(colorAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, ColorUpdateDto colorUpdateDto)
        {
            if (id != colorUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen renk ID'si eşleşmiyor!");
            }

            var result = await _adminColorService.UpdateAsync(colorUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminColorService.DeleteAsync(id);
            return Ok(result);
        }
    }
}   
