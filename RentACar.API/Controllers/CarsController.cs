using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }
        // TODO: Mobili kodlarken interface te olan daha kodlanmış markaya ve renge göre araçları getirme metodunu buraya ekle.
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _carService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _carService.GetByIdAsync(id);
            return Ok(result);
        }
    }
}
