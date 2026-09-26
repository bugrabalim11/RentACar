using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class CarMaintenancesController : ControllerBase
    {
        private readonly ICarMaintenanceService _carMaintenanceService;

        public CarMaintenancesController(ICarMaintenanceService carMaintenanceService)
        {
            _carMaintenanceService = carMaintenanceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CarMaintenanceCreateDto carMaintenanceAddDto)
        {
            var result = await _carMaintenanceService.AddAsync(carMaintenanceAddDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _carMaintenanceService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _carMaintenanceService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _carMaintenanceService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CarMaintenanceUpdateDto carMaintenanceUpdateDto)
        {
            if (id != carMaintenanceUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen tamir ID'si eşleşmiyor!");
            }

            var result = await _carMaintenanceService.UpdateAsync(carMaintenanceUpdateDto);
            return Ok(result);
        }
    }
}
