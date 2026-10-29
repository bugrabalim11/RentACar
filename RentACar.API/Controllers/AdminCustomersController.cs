using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.CustomerDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/customers")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminCustomersController : ControllerBase
    {
        private readonly IAdminCustomerService _adminCustomerService;

        public AdminCustomersController(IAdminCustomerService adminCustomerService)
        {
            _adminCustomerService = adminCustomerService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GelAllAsync()
        {
            var result = await _adminCustomerService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _adminCustomerService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateByAdminAsync(CustomerCreateByAdminDto customerAddByAdminDto)
        {
            var result = await _adminCustomerService.AddForAdminAsync(customerAddByAdminDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateByAdminAsync(int id, CustomerUpdateByAdminDto customerUpdateDto)
        {
            if (id != customerUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen müşteri ID'si eşleşmiyor!");
            }

            var result = await _adminCustomerService.UpdateAsync(customerUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminCustomerService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
