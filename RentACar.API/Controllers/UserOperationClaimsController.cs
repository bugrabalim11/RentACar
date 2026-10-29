using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Extensions;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserOperationClaimsController : ControllerBase
    {
        private readonly IUserOperationClaimService _userOperationClaimService;

        public UserOperationClaimsController(IUserOperationClaimService userOperationClaimService)
        {
            _userOperationClaimService = userOperationClaimService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await _userOperationClaimService.GetAllAsync();
            return Ok(results);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)   
        {
            var result = await _userOperationClaimService.GetByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("details")]
        public async Task<IActionResult> GetClaimDetailsAsync()
        {
            var result = await _userOperationClaimService.GetClaimDetailsAsync();
            return Ok(result);
        }

        [HttpGet("my-claims")]
        public async Task<IActionResult> GetMyClaimsAsync()
        {
            int userId = User.GetUserId();
            var result = await _userOperationClaimService.GetMyOperationClaimsAsync(userId);
            return Ok(result);
        }
    }
}