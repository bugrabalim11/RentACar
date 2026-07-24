using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.OfficeDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeService _officeService;

        public OfficesController(IOfficeService officeService)
        {
            _officeService = officeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _officeService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _officeService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Add(OfficeAddDto officeAddDto)
        {
            var result = await _officeService.AddAsync(officeAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update(OfficeUpdateDto officeUpdateDto)
        {
            var result = await _officeService.UpdateAsync(officeUpdateDto);
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
            var result = await _officeService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
