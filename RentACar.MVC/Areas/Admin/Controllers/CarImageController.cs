using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.CarImageDtos;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.DeleteAsync($"api/CarImages/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Json(new { success = false, message = "Bu işlem için yetkiniz yok. Lütfen giriş yapın!" });
            }
            return Json(new { success = false, message = "Api tarafından silme işlemi başarısız oldu!" });
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

        [HttpPost]
        public async Task<IActionResult> Create(CarImageCreateDto carImageCreateDto)
        {
            // 1. GARSONUN ÖN KONTROLÜ: Eğer DTO'daki [Required] kuralları ihlal edildiyse (resim seçilmediyse),
            // kuryeyi (HttpClient) hiç yola çıkarmadan masadaki formu (View) hatalarıyla geri döndür.
            if (!ModelState.IsValid)
            {
                return View(carImageCreateDto);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            // SENIOR NOTU: using kelimesi, bu devasa kargo kolisi (MultipartFormDataContent) API'ye 
            // ulaştığı an bekleme yapmadan RAM'den silinmesini (Garbage Collector) sağlar.
            using var content = new MultipartFormDataContent();

            // Arabanın ID'sini koliye bir kağıt (StringContent) olarak ekliyoruz.
            content.Add(new StringContent(carImageCreateDto.CarId.ToString()), "CarId");

            // Resmi parça parça akış (Stream) olarak koliye ekliyoruz.
            var streamContent = new StreamContent(carImageCreateDto.ImageFile.OpenReadStream());
            // SENIOR NOTU: Kuryenin (HttpClient) taşıdığı fiziksel koliye dijital bir etiket (MIME type) basıyoruz.
            // Eğer bunu yazmazsak, C# koliye varsayılan olarak "Bilinmeyen Dijital Yük" (application/octet-stream) etiketi basar.
            // Mutfak (API) kargoyu açıp bu etiketi gördüğünde, FluentValidation kurallarımız "Bu bir resim değil!" diyerek paketi reddeder.
            // Bu yüzden HTML'den gelen orijinal etiketini (örn: image/jpeg) koliye aynen kopyalıyoruz.
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(carImageCreateDto.ImageFile.ContentType);
            content.Add(streamContent, "ImageFile", carImageCreateDto.ImageFile.FileName);

            var responseMessage = await client.PostAsync("api/CarImages", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                // Başarılıysa müşteriyi albüm odasına yolla
                return RedirectToAction("Index", new { carId = carImageCreateDto.CarId });
            }

            // API'den hata dönerse (örn: 5MB sınırı aşıldıysa) aynı sayfada kal
            var errorMessage = await responseMessage.Content.ReadAsStringAsync();
            return View(carImageCreateDto);
        }
    }
}

