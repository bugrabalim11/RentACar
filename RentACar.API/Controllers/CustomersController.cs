using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Extensions;
using RentACar.Dtos.CustomerDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyCustomerProfileAsync()
        {
            int userId = User.GetUserId();
            var result = await _customerService.GetMyCustomerProfileAsync(userId);
            return Ok(result);
        }

        [HttpPost("profile")]
        public async Task<IActionResult> CreateAsync(CustomerCreateDto customerAddDto)
        {
            int userId = User.GetUserId();
            var result = await _customerService.AddAsync(userId, customerAddDto);
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateAsync(CustomerUpdateMyProfileDto customerUpdateMyProfileDto)
        {
            int userId = User.GetUserId();
            var result = await _customerService.UpdateMyProfileAsync(userId, customerUpdateMyProfileDto);
            return Ok(result);
        }
    }
}
