using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/car-maintenances")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminCarMaintenancesController : ControllerBase
    {
        private readonly IAdminCarMaintenanceService _adminCarMaintenanceService;

        public AdminCarMaintenancesController(IAdminCarMaintenanceService adminCarMaintenanceService)
        {
            _adminCarMaintenanceService = adminCarMaintenanceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CarMaintenanceCreateDto carMaintenanceAddDto)
        {
            var result = await _adminCarMaintenanceService.AddAsync(carMaintenanceAddDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]    
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminCarMaintenanceService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CarMaintenanceUpdateDto carMaintenanceUpdateDto)
        {
            if (id != carMaintenanceUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen tamir ID'si eşleşmiyor!");
            }

            var result = await _adminCarMaintenanceService.UpdateAsync(carMaintenanceUpdateDto);
            return Ok(result);
        }
    }
}
