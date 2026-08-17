using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.CustomerDtos;
using System.Security.Claims;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GelAllAsync()
        {
            var result = await _customerService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _customerService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyCustomerProfileAsync()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Kimlik doğrulama hatası!");
            int userId = Convert.ToInt32(userIdString);

            var result = await _customerService.GetMyCustomerProfileAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CustomerAddDto customerAddDto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Kimlik doğrulama hatası!");
            int userId = Convert.ToInt32(userIdString);

            var result = await _customerService.AddAsync(userId, customerAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateAsync(CustomerUpdateMyProfileDto customerUpdateMyProfileDto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Kimlik doğrulama hatası!");
            int userId = Convert.ToInt32(userIdString);

            var result = await _customerService.UpdateMyProfileAsync(userId, customerUpdateMyProfileDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForAdmin(int id, CustomerUpdateDto customerUpdateDto)
        {
            // Senior Vizyonu: İstek (Request) tutarlılık kontrolü (Controller'ın görevi).
            // URL'deki kapı numarası ile DTO (Kargo paketi) içindeki ID eşleşiyor mu?
            if (id != customerUpdateDto.Id)
            {
                return BadRequest("URL'deki ID ile gönderilen müşteri ID'si eşleşmiyor!");
            }

            var result = await _customerService.UpdateAsync(customerUpdateDto);
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
            var result = await _customerService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
