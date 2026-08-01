using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.ColorDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _colorService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _colorService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddAsync(ColorAddDto colorAddDto)
        {
            var result = await _colorService.AddAsync(colorAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(ColorUpdateDto colorUpdateDto)
        {
            var result = await _colorService.UpdateAsync(colorUpdateDto);
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
            var result = await _colorService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
