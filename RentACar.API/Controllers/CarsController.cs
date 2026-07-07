using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<IActionResult> AddCar(CarAddDto carAddDto)
        {
            // İşi aşçıya (Business katmanına) devrediyoruz
            await _carService.AddAsync(carAddDto);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Aşçıdan tabakları (DTO listesini) istiyoruz
            var result = await _carService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _carService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
