using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.CarDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/cars")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminCarsController : ControllerBase   
    {
        private readonly IAdminCarService _adminCarService;

        public AdminCarsController(IAdminCarService adminCarService)
        {
            _adminCarService = adminCarService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CarCreateDto carAddDto)
        {
            var result = await _adminCarService.AddAsync(carAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CarUpdateDto carUpdateDto)
        {
            if (id != carUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen araç ID'si eşleşmiyor!");
            }
            var result = await _adminCarService.UpdateAsync(carUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminCarService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
