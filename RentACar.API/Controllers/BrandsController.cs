using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetAll()
        {
            var values = await _brandService.GetAllAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await _brandService.GetByIdAsync(id);
            if (value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> AddBrand(BrandAddDto brandAddDto)
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

        [HttpPut]
        public async Task<IActionResult> UpdateBrand(BrandUpdateDto brandUpdateDto)
        {
            var result = await _brandService.UpdateAsync(brandUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var result = await _brandService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
