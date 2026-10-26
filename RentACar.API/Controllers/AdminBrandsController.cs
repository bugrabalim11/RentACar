using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.BrandDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/brands")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminBrandsController : ControllerBase
    {
        private readonly IAdminBrandService _adminBrandService;

        public AdminBrandsController(IAdminBrandService adminBrandService)
        {
            _adminBrandService = adminBrandService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(BrandCreateDto brandAddDto)
        {
            var result = await _adminBrandService.AddAsync(brandAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, BrandUpdateDto brandUpdateDto)
        {
            // GÜVENLİK DUVARI: Garson ilk URL ve Form uyuşmazlığını yakalarsa kırmızı alarma basar!
            if (id != brandUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen marka ID'si eşleşmiyor!");
            }

            var result = await _adminBrandService.UpdateAsync(brandUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminBrandService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
