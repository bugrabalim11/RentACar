using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.BrandDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _brandService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);  // Kutuyu içindeki "data" listesiyle beraber aynen döner
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _brandService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result); // Kutuyu içindeki tek bir "data" (Brand) nesnesiyle döner
            }
            return NotFound(result);
        }

        [Authorize(Roles = "admin")] 
        // veya [Authorize(Roles = "Admin,Moderator")] şeklinde virgülle çoklu rütbe de verebilirsin.
        [HttpPost]
        public async Task<IActionResult> AddAsync(BrandAddDto brandAddDto)
        {
            var result = await _brandService.AddAsync(brandAddDto);

            // Kutunun üzerindeki Başarı (Success) etiketini kontrol et
            if (result.Success)
            {
                return Ok(result);
            }

            // İşlem başarısızsa, müşteriye 400 Bad Request ile kutuyu ver
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, BrandUpdateDto brandUpdateDto)
        {
            if (id != brandUpdateDto.Id)
            {
                return BadRequest("Güvenlik İhlali: URL'deki ID ile gönderilen marka ID'si eşleşmiyor!");
            }

            var result = await _brandService.UpdateAsync(brandUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _brandService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
