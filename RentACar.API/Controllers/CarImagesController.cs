using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.CarImageDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class CarImagesController : ControllerBase
    {
        private readonly ICarImageService _carImageService;

        public CarImagesController(ICarImageService carImageService)
        {
            _carImageService = carImageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CarImageCreateDto carImageAddDto)
        {
            var result = await _carImageService.AddAsync(carImageAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] CarImageUpdateDto carImageUpdateDto)
        {
            if (id != carImageUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen resim ID'si eşleşmiyor!");
            }

            var result = await _carImageService.UpdateAsync(carImageUpdateDto);
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _carImageService.DeleteAsync(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("car/{carId}")]
        public async Task<IActionResult> GetImagesByCarIdAsync(int carId)
        {
            var result = await _carImageService.GetImagesByCarIdAsync(carId);
            return Ok(result);
        }
    }
}
