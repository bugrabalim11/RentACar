using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CarImagesController : ControllerBase
    {
        private readonly ICarImageService _carImageService;

        public CarImagesController(ICarImageService carImageService)
        {
            _carImageService = carImageService;
        }

        [HttpGet("cars/{carId}")]
        public async Task<IActionResult> GetImagesByCarIdAsync(int carId)
        {
            var result = await _carImageService.GetImagesByCarIdAsync(carId);
            return Ok(result);
        }
    }
}
