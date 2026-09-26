using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.BrandDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            // SİPARİŞ: Garson listeyi ister, mutfak sorunsuz dönerse müşteriye (200 OK) iletir.
            var result = await _brandService.GetAllAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _brandService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(BrandCreateDto brandAddDto)
        {
            var result = await _brandService.AddAsync(brandAddDto);
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

            var result = await _brandService.UpdateAsync(brandUpdateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _brandService.DeleteAsync(id);
            return Ok(result);
        }
    }
}