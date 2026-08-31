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
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Brands");
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
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.DeleteAsync($"api/Brands/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Json(new { success = false, message = "Bu işlem için yetkiniz yok. Lütfen giriş yapın!" });
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
        public async Task<IActionResult> Create(BrandCreateDto brandCreateDto)
        {
            // 1. GÜVENLİK KALKANI: Müşteri formu boş mu gönderdi? Kuralları (Örn: Required) ihlal etti mi?
            // Eğer form hatalıysa hiç API'ye gitme, müşteriye formu geri ver.
            // Sadece şu kısmı yanlış yazmışsın, düzelt" diyerek üzerindeki eski verilerle birlikte müşteriye geri verir.
            if (!ModelState.IsValid) { return View(brandCreateDto); }

            // 2. KURYE ÇAĞIR: API mutfağına gidecek garsonumuzu (HttpClient) hazırlıyoruz.
            var client = _httpClientFactory.CreateClient("RentACarApi");

            // 3. ÇEVİRMEN (SERIALIZE): C# dilindeki nesnemizi, mutfağın anladığı evrensel dile (JSON) çeviriyoruz.
            var jsonData = JsonConvert.SerializeObject(brandCreateDto);

            // 4. ZARF VE PUL (StringContent): Çıplak JSON yollanmaz! Onu zarfa koyup dilinin (UTF-8)
            // ve türünün (application/json) ne olduğunu gümrük memuruna (HTTP Protokolüne) bildiriyoruz.
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // 5. YOLA ÇIKIŞ: Kurye, elindeki zarfla API'nin kapısına POST (Veri Ekleme) isteği atıyor.
            var responseMessage = await client.PostAsync("api/Brands", stringContent);

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

            // brandCreateDto dödürdük ki hata varsa hepsini tekrar yazmasın 
            return View(brandCreateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            // 1. Kurye mutfağa (API'nin GetById kapısına) gidiyor:
            var responseMessasge = await client.GetAsync($"api/Brands/{id}");

            // 2. Mutfaktan tabak geldiyse:
            if (responseMessasge.IsSuccessStatusCode)
            {
                // Kutuyu açıp içindeki JSON'u okuyoruz
                var jsonData = await responseMessasge.Content.ReadAsStringAsync();

                // 1. Önce koca koliyi (Matruşkanın tamamını) çözüyoruz
                var response = JsonConvert.DeserializeObject<GetByIdBrandResponseDto>(jsonData);

                // 2. Form (View) bizden koca koliyi değil, sadece içindeki arabayı (Id ve Name) bekliyor!
                // Bu yüzden View'a sadece response içindeki Data'yı gönderiyoruz.
                if (response != null && response.Data != null)
                {
                    return View(response.Data);
                }
            }

            // Eğer o Id'de bir marka yoksa listeye geri yolla
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(BrandUpdateDto brandUpdateDto)
        {
            if (!ModelState.IsValid) { return View(brandUpdateDto); } 

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(brandUpdateDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/Brands/{brandUpdateDto.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Eğer mutfak bizi direkt kapıdan kovduysa (Giriş yapmamışsak)
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                return View(brandUpdateDto);
            }
            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);
            if (errorData != null)
            {
                ModelState.AddModelError(string.Empty, errorData.Message);
            }
            return View(brandUpdateDto);
        }
    }
}
