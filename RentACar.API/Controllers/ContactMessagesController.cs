using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.Dtos.ContactMessageDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessagesController : ControllerBase
    {
        private readonly IContactMessageService _contactMessageService;

        public ContactMessagesController(IContactMessageService contactMessageService)
        {
            _contactMessageService = contactMessageService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contactMessageService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _contactMessageService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ContactMessageAddDto contactMessageAddDto)
        {
            var result = await _contactMessageService.AddAsync(contactMessageAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactMessageService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("ChangeIsReadStatus/{id}")]
        public async Task<IActionResult> ChangeIsReadStatusAsync(int id)
        {
            var result = await _contactMessageService.ChangeIsReadStatusAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
