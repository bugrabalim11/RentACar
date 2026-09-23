using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
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
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _customerService.GetByIdAsync(id);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyCustomerProfileAsync()
        {
            // SENIOR VİZYONU: Amelelik bitti, tek satırda kimliği cüzdandan çekiyoruz!
            int userId = GetUserIdFromClaims();

            var result = await _customerService.GetMyCustomerProfileAsync(userId);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateByAdminAsync(CustomerCreateByAdminDto customerAddByAdminDto)
        {
            var result = await _customerService.AddForAdminAsync(customerAddByAdminDto);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("profile")]
        public async Task<IActionResult> CreateAsync(CustomerCreateDto customerAddDto)
        {
            int userId = GetUserIdFromClaims();
            var result = await _customerService.AddAsync(userId, customerAddDto);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateAsync(CustomerUpdateMyProfileDto customerUpdateMyProfileDto)
        {
            int userId = GetUserIdFromClaims();
            var result = await _customerService.UpdateMyProfileAsync(userId, customerUpdateMyProfileDto);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateByAdminAsync(int id, CustomerUpdateByAdminDto customerUpdateDto)
        {
            // İstek tutarlılık kontrolü: URL'deki kapı numarası ile kargo paketi (DTO) eşleşiyor mu?
            if (id != customerUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen müşteri ID'si eşleşmiyor!");
            }

            var result = await _customerService.UpdateAsync(customerUpdateDto);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _customerService.DeleteAsync(id);
            return Ok(result);
        }

        // --- YARDIMCI METOTLAR (Sadece bu Controller'ın iç kullanımı için) ---

        // DRY Prensibi: Cüzdandan (Token) ID okuma işlemini tek bir merkeze topladık.
        private int GetUserIdFromClaims()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new BusinessException("Kimlik doğrulama hatası! Geçerli bir token bulunamadı!");
            }
            return Convert.ToInt32(userIdString);
        }
    }
}
