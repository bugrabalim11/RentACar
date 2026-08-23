using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class CarMaintenanceController : ControllerBase
    {
        private readonly ICarMaintenanceService _carMaintenanceService;

        public CarMaintenanceController(ICarMaintenanceService carMaintenanceService)
        {
            _carMaintenanceService = carMaintenanceService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CarMaintenanceAddDto carMaintenanceAddDto)
        {
            var result = await _carMaintenanceService.AddAsync(carMaintenanceAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _carMaintenanceService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _carMaintenanceService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _carMaintenanceService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CarMaintenanceUpdateDto carMaintenanceUpdateDto)
        {
            if (id != carMaintenanceUpdateDto.Id)
            {
                return BadRequest("Güvenlik İhlali: URL'deki ID ile gönderilen tamir ID'si eşleşmiyor!");
            }

            var result = await _carMaintenanceService.UpdateAsync(carMaintenanceUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
