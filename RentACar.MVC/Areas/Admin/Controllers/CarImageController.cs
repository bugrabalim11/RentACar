using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.CarImageDtos;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "admin")]
    public class CarImageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CarImageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int carId)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/CarImages/Car/{carId}");
            // SENIOR NOTU: Kullanıcı 'Yeni Resim Ekle' dediğinde hangi arabaya ekleme yapacağımızı
            // bilmek için bu ID'yi HTML'e ufak bir Post-it notu olarak yolluyoruz.
            ViewBag.CarId = carId;
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<CarImageResponseDto>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<CarImageResultDto>());
        }

        [HttpGet]
        public IActionResult Create(int carId)
        {
            // SENIOR NOTU: Müşterinin masasına (View) eli boş gitmiyoruz.
            // HTML'deki 'asp-for' etiketlerinin patlamaması için masaya boş bir sipariş fişi (DTO) bırakıyoruz.
            // Aynı zamanda fişin üzerine gizlice hangi arabaya (CarId) ait olduğunu damgalıyoruz ki POST işleminde kaybolmasın.
            CarImageCreateDto carImageCreateDto = new CarImageCreateDto
            {
                CarId = carId,
            };
            return View(carImageCreateDto);
        }
    }
}

