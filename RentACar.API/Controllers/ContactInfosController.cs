using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.ContactInfoDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ContactInfosController : ControllerBase
    {
        private readonly IContactInfoService _contactInfoService;

        public ContactInfosController(IContactInfoService contactInfoService)
        {
            _contactInfoService = contactInfoService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _contactInfoService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _contactInfoService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ContactInfoAddDto contactInfoAddDto)
        {
            var result = await _contactInfoService.AddAsync(contactInfoAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, ContactInfoUpdateDto contactInfoUpdateDto)
        {
            if (id != contactInfoUpdateDto.Id)
            {
                return BadRequest("Güvenlik İhlali: URL'deki ID ile gönderilen marka ID'si eşleşmiyor!");
            }

            var result = await _contactInfoService.UpdateAsync(contactInfoUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _contactInfoService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
