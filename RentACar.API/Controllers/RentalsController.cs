using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.RentalDtos;
using System.Security.Claims;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddByAdminAsync(RentalAddByAdminDto rentalAddByAdminDto)
        {
            var result = await _rentalService.AddByAdminAsync(rentalAddByAdminDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPost("rental")]
        public async Task<IActionResult> AddAsync(RentalAddDto rentalAddDto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Kimlik doğrulama hatası!");
            int userId = Convert.ToInt32(userIdString);

            var result = await _rentalService.AddAsync(rentalAddDto, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _rentalService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("rentals")]
        public async Task<IActionResult> GetMyRentalsAsync()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Kimlik doğrulama hatası!");
            int userId = Convert.ToInt32(userIdString);

            var result = await _rentalService.GetAllByUserIdAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("rental/{rentalId}")]
        public async Task<IActionResult> GetMyRentalByIdAsync(int rentalId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Kimlik doğrulama hatası!");
            int userId = Convert.ToInt32(userIdString);

            var result = await _rentalService.GetMyRentalByIdAsync(rentalId, userId);
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
            var result = await _rentalService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, RentalUpdateDto rentalUpdateDto)
        {
            if (id != rentalUpdateDto.Id)
            {
                return BadRequest("Güvenlik İhlali: URL'deki ID ile gönderilen kiralama ID'si eşleşmiyor!");
            }

            var result = await _rentalService.UpdateAsync(rentalUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPut("rental/{rentalId}")]
        public async Task<IActionResult> UpdateMyRentalAsync(int rentalId, RentalUpdateReturnDateDto rentalUpdateReturnDateDto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Kimlik doğrulama hatası!");
            int userId = Convert.ToInt32(userIdString);

            var result = await _rentalService.UpdateMyRentalAsync(userId, rentalId, rentalUpdateReturnDateDto);
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
            var result = await _rentalService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
