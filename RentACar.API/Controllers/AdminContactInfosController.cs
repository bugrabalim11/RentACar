using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.ContactInfoDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminContactInfosController : ControllerBase
    {
        private readonly IAdminContactInfoService _adminContactInfoService;

        public AdminContactInfosController(IAdminContactInfoService adminContactInfoService)
        {
            _adminContactInfoService = adminContactInfoService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ContactInfoCreateDto contactInfoAddDto)
        {
            var result = await _adminContactInfoService.AddAsync(contactInfoAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, ContactInfoUpdateDto contactInfoUpdateDto)
        {
            if (id != contactInfoUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen iletişim bilgisi ID'si eşleşmiyor!");
            }

            var result = await _adminContactInfoService.UpdateAsync(contactInfoUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminContactInfoService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
