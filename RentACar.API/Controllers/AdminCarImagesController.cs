using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Dtos.CarImageDtos;

namespace RentACar.API.Controllers
{
    [Route("api/admin/carimages")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminCarImagesController : ControllerBase
    {
        private readonly IAdminCarImageService _adminCarImageService;

        public AdminCarImagesController(IAdminCarImageService adminCarImageService)
        {
            _adminCarImageService = adminCarImageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CarImageCreateDto carImageAddDto)
        {
            var result = await _adminCarImageService.AddAsync(carImageAddDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] CarImageUpdateDto carImageUpdateDto)
        {
            if (id != carImageUpdateDto.Id)
            {
                throw new BusinessException("Güvenlik İhlali: URL'deki ID ile gönderilen resim ID'si eşleşmiyor!");
            }

            var result = await _adminCarImageService.UpdateAsync(carImageUpdateDto);
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _adminCarImageService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("cars/{carId}/deleted")]
        public async Task<IActionResult> GetDeletedImagesByCarIdAsync(int carId)
        {
            var result = await _adminCarImageService.GetDeletedImagesByCarIdAsync(carId);
            return Ok(result);
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> RestoreAsync(int id)
        {
            var result = await _adminCarImageService.RestoreAsync(id);
            return Ok(result);
        }
    }
}
