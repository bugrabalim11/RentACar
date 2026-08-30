using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.BrandDtos;
using RentACar.MVC.Areas.Admin.Models.ErrorResponseDtos;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BrandController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var responseMessage = await client.GetAsync("https://localhost:7085/api/Brands");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<BrandResponseDto>(jsonData);
                if (responseBox != null)
                {
                    // API'nin gönderdiği dış kutunun içindeki asıl marka listesini (Tepsiyi) masaya servis ediyoruz.
                    // Yani başaralı mesajlarını falan değil direkt veriyi
                    return View(responseBox.Data);
                }
            }
            // API çağrısı başarısız olursa, sayfa patlamasın diye boş dönüyoruz.
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var responseMessage = await client.DeleteAsync($"https://localhost:7085/api/Brands/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "API tarafında silme işlemi başarısız oldu!" });
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Burada API'ye gitmiyoruz! Sadece boş sipariş fişini (View) masaya bırakıyoruz.
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandDto createBrandDto)
        {
            // 1. GÜVENLİK KALKANI: Müşteri formu boş mu gönderdi? Kuralları (Örn: Required) ihlal etti mi?
            // Eğer form hatalıysa hiç API'ye gitme, müşteriye boş formu geri ver.
            if (!ModelState.IsValid) { return View(); }

            // 2. KURYE ÇAĞIR: API mutfağına gidecek garsonumuzu (HttpClient) hazırlıyoruz.
            var client = _httpClientFactory.CreateClient();

            // 3. ÇEVİRMEN (SERIALIZE): C# dilindeki nesnemizi, mutfağın anladığı evrensel dile (JSON) çeviriyoruz.
            var jsonData = JsonConvert.SerializeObject(createBrandDto);

            // 4. ZARF VE PUL (StringContent): Çıplak JSON yollanmaz! Onu zarfa koyup dilinin (UTF-8)
            // ve türünün (application/json) ne olduğunu gümrük memuruna (HTTP Protokolüne) bildiriyoruz.
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // 5. YOLA ÇIKIŞ: Kurye, elindeki zarfla API'nin kapısına POST (Veri Ekleme) isteği atıyor.
            var responseMessage = await client.PostAsync("https://localhost:7085/api/Brands", stringContent);

            // 6. MUTLU SON KONTROLÜ: API "Tamamdır, başarıyla ekledim" dedi mi?
            if (responseMessage.IsSuccessStatusCode)
            {
                // Başarılıysa kurye bizi tabloya (Marka Listesine) geri yollar.
                return RedirectToAction("Index");
            }

            // 1. ZARFI AÇ VE OKU (ReadAsStringAsync): Mutfaktan gelen kızgın notu (JSON) metin olarak okuyoruz.
            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();

            // 2. ÇEVİRMEN (Deserialize): Okuduğumuz JSON notunu, az önce yaptığımız Çevik Kuryeye (ErrorResponseDto) dönüştürüyoruz.
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);
            if (errorData != null)
            {
                // 3. MÜŞTERİYE NOT YAPIŞTIR (ModelState.AddModelError):
                // Kurye boş sipariş fişini müşteriye geri vermeden önce, formun üzerine kırmızı bir not yapıştırıyor!
                ModelState.AddModelError(string.Empty, errorData.Message);
            }

            // Sonra da bu kırmızı not yapıştırılmış formu müşterinin yüzüne tekrar gösteriyoruz.
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // 1. Kurye mutfağa (API'nin GetById kapısına) gidiyor:
            var responseMessasge = await client.GetAsync($"https://localhost:7085/api/Brands/{id}");

            // 2. Mutfaktan tabak geldiyse:
            if (responseMessasge.IsSuccessStatusCode)
            {
                // Kutuyu açıp içindeki JSON'u okuyoruz
                var jsonData = await responseMessasge.Content.ReadAsStringAsync();

                // ÇEVİRMEN (Deserialize): JSON'u bizim yeni çantaya (UpdateBrandDto) koyuyoruz.
                var values = JsonConvert.DeserializeObject<UpdateBrandDto>(jsonData);

                // Bu dolu çantayı View'a (Arayüze) gönderiyoruz ki form ekranda dolu gelsin!
                return View(values);
            }

            // Eğer o Id'de bir marka yoksa listeye geri yolla
            return RedirectToAction("Index");
        }
    }
}
