using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.RentalDtos;
using System.Security.Claims;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateByAdminAsync(RentalCreateByAdminDto rentalAddByAdminDto)
        {
            var result = await _rentalService.AddByAdminAsync(rentalAddByAdminDto);
            return Ok(result);
        }

        [HttpPost("rental")]
        public async Task<IActionResult> CreateAsync(RentalCreateDto rentalAddDto)
        {
            int userId = GetUserIdFromClaims();
            var result = await _rentalService.AddAsync(rentalAddDto, userId);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _rentalService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("rentals")]
        public async Task<IActionResult> GetMyRentalsAsync()
        {
            int userId = GetUserIdFromClaims();
            var result = await _rentalService.GetAllByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("{rentalId}/rental")]
        public async Task<IActionResult> GetMyRentalByIdAsync(int rentalId)
        {
            int userId = GetUserIdFromClaims();
            var result = await _rentalService.GetMyRentalByIdAsync(rentalId, userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _rentalService.GetByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateByAdminAsync(int id, RentalUpdateByAdminDto rentalUpdateDto)
        {
            if (id != rentalUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen kiralama ID'si eşleşmiyor!");
            }
            var result = await _rentalService.UpdateByAdminAsync(rentalUpdateDto);
            return Ok(result);
        }

        [HttpPut("{rentalId}/rental")]
        public async Task<IActionResult> UpdateMyRentalAsync(int rentalId, RentalUpdateReturnDateDto rentalUpdateReturnDateDto)
        {
            int userId = GetUserIdFromClaims();
            var result = await _rentalService.UpdateMyRentalAsync(userId, rentalId, rentalUpdateReturnDateDto);
            return Ok(result);
        }

        [Authorize(Roles ="admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _rentalService.DeleteAsync(id);
            return Ok(result);
        }

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
