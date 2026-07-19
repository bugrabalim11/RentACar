using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.ContactInfoDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfosController : ControllerBase
    {
        private readonly IContactInfoService _contactInfoService;

        public ContactInfosController(IContactInfoService contactInfoService)
        {
            _contactInfoService = contactInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contactInfoService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _contactInfoService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ContactInfoAddDto contactInfoAddDto)
        {
            var result = await _contactInfoService.AddAsync(contactInfoAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ContactInfoUpdateDto contactInfoUpdateDto)
        {
            var result = await _contactInfoService.UpdateAsync(contactInfoUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
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
