using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;

namespace RentACar.API.Controllers
{
    [Route("api/admin/contact-messages")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminContactMessagesController : ControllerBase
    {
        private readonly IAdminContactMessageService _adminContactMessageService;

        public AdminContactMessagesController(IAdminContactMessageService adminContactMessageService)
        {
            _adminContactMessageService = adminContactMessageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _adminContactMessageService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _adminContactMessageService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminContactMessageService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPatch("MarkAsRead/{id}")]
        public async Task<IActionResult> MarkAsReadAsync(int id)
        {
            var result = await _adminContactMessageService.MarkAsReadAsync(id);
            return Ok(result);
        }
    }
}
