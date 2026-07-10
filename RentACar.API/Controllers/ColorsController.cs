using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetAll()
        {
            var values = await _colorService.GetAllAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await _colorService.GetByIdAsync(id);
            if (value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> AddColor(ColorAddDto colorAddDto)
        {
            await _colorService.AddAsync(colorAddDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateColor(ColorUpdateDto colorUpdateDto)
        {
            var result = await _colorService.UpdateAsync(colorUpdateDto);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var result = await _colorService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
