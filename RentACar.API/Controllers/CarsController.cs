using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.CarDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddAsync(CarAddDto carAddDto)
        {
            // İşi aşçıya (Business katmanına) devrediyoruz
            var result = await _carService.AddAsync(carAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            // Aşçıdan tabakları (DTO listesini) istiyoruz
            var result = await _carService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _carService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(CarUpdateDto carUpdateDto)
        {
            var result = await _carService.UpdateAsync(carUpdateDto);
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
            var result = await _carService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
