using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;

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
    }
}
