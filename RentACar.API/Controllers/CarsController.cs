using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentACar.Business.Abstract;
using RentACar.Dtos.CarDtos;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCar(CarAddDto carAddDto)
        {
            // İşi aşçıya (Business katmanına) devrediyoruz
            await _carService.AddAsync(carAddDto);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Aşçıdan tabakları (DTO listesini) istiyoruz
            var result = await _carService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _carService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCar(CarUpdateDto carUpdateDto)
        {
            // 1. GetByIdAsync'i SİLİYORUZ! Doğrudan UpdateAsync'i çağırıyoruz.
            // Çünkü UpdateAsync metodu geriye 'bool' (true/false) dönecek (Yeni yazdığımız düzende).
            var result = await _carService.UpdateAsync(carUpdateDto);

            // 2. Artık result bir 'bool' olduğu için önüne rahatça '!' koyabiliriz!
            if (!result) // Yani: "Eğer güncelleme başarısızsa (false döndüyse)"
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var result = await _carService.DeleteAsync(id);

            if (!result)  // Eğer sonuç 'false' döndüyse
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
