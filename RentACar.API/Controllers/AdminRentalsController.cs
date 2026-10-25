using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.RentalDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/rentals")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminRentalsController : ControllerBase
    {
        private readonly IAdminRentalService _adminRentalService;

        public AdminRentalsController(IAdminRentalService adminRentalService)
        {
            _adminRentalService = adminRentalService;
        }
  
        [HttpPost]
        public async Task<IActionResult> CreateByAdminAsync(RentalCreateByAdminDto rentalAddByAdminDto)
        {
            var result = await _adminRentalService.AddByAdminAsync(rentalAddByAdminDto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _adminRentalService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _adminRentalService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateByAdminAsync(int id, RentalUpdateByAdminDto rentalUpdateDto)
        {
            if (id != rentalUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen kiralama ID'si eşleşmiyor!");
            }
            var result = await _adminRentalService.UpdateByAdminAsync(rentalUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminRentalService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
