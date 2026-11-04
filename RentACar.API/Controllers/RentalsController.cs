using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Extensions;
using RentACar.Dtos.RentalDtos;

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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(RentalCreateDto rentalAddDto)
        {
            int userId = User.GetUserId();
            var result = await _rentalService.AddAsync(rentalAddDto, userId);
            return Ok(result);
        }

        [HttpGet("my-rentals")]
        public async Task<IActionResult> GetMyRentalsAsync()
        {
            int userId = User.GetUserId();
            var result = await _rentalService.GetAllByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("my-rentals/{id}")]
        public async Task<IActionResult> GetMyRentalByIdAsync(int id)
        {
            int userId = User.GetUserId();
            var result = await _rentalService.GetMyRentalByIdAsync(id, userId);
            return Ok(result);
        }

        [HttpPut("my-rentals/{id}")]
        public async Task<IActionResult> UpdateMyRentalAsync(int id, RentalUpdateReturnDateDto rentalUpdateReturnDateDto)
        {
            int userId = User.GetUserId();
            var result = await _rentalService.UpdateMyRentalAsync(userId, id, rentalUpdateReturnDateDto);
            return Ok(result);
        }
    }
}