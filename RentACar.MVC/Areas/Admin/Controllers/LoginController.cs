using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.AuthDtos;
using RentACar.MVC.Areas.Admin.Models.ErrorResponseDtos;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserForLoginDto userForLoginDto)
        {
            // 1. ZIRH: Kullanıcı formu eksik veya hatalı mı doldurdu?
            if (!ModelState.IsValid) { return View(userForLoginDto); }

            // 2. KURYE: API mutfağına gidecek kuryeyi çağırıyoruz.
            var client = _httpClientFactory.CreateClient("RentACarApi");

            // 3. PAKETLEME: Gelen form bilgilerini API'nin anladığı JSON diline çevirip zarflıyoruz.
            var jsonData = JsonConvert.SerializeObject(userForLoginDto);
            var stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            // 4. GÖNDERİM: Kurye zarfı alıp API'nin Login kapısına (POST) gidiyor.
            var responseMessage = await client.PostAsync("api/Auths/Login", stringContent);

            // 5. KAZA KONTROLÜ (BAŞARISIZLIK): Eğer kurye kapıdan kovulursa (Örn: Şifre yanlış)
            if (!responseMessage.IsSuccessStatusCode)
            {
                // API'den dönen kırmızı hata notunu oku ve bizim Hata Kalıbına (ErrorResponseDto) dök.
                var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
                var errrorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);

                if (errrorData != null)
                {
                    // Müşteriye formu silmeden, üzerine kırmızı hata notunu yapıştırarak geri ver.
                    ModelState.AddModelError(string.Empty, errrorData.Message);
                    return View(userForLoginDto);
                }
            }

            // 6. ZAFER (BAŞARI): Kod buraya kadar takılmadan ulaştıysa, kurye başarılı dönmüş (200 OK) demektir!
            // Kuryenin getirdiği başarılı paketi (JSON) oku.
            var successJsonData = await responseMessage.Content.ReadAsStringAsync();

            // Paketin içindeki Token bilgisini az önce ellerinle açtığın DTO kalıbına dök.
            var tokenData = JsonConvert.DeserializeObject<AccessTokenResponseDto>(successJsonData);
            if (tokenData == null)
            {
                ModelState.AddModelError(string.Empty, "Token bilgisi alınamadı.");
                return View(userForLoginDto);
            }

            // 7. CÜZDAN (COOKIE): Kuryenin getirdiği VIP kartını (Token) tarayıcının kilitli cüzdanına koyuyoruz.
            Response.Cookies.Append("AccessToken", tokenData.Token, new CookieOptions
            {
                HttpOnly = true, // JavaScript ile erişilemez (Hacker Kalkanı)
                SameSite = SameSiteMode.Strict, // Sadece bizim sitemizden istek atılırsa gider
                Secure = false, // Geliştirme aşamasında false, Canlıya (Production) alırken KESİNLİKLE true olacak!
                Expires = DateTime.Now.AddDays(1) // 1 gün sonra cüzdandan silinir
            });

            // 8. İÇERİ GİRİŞ: Cüzdanına kartı koyduğumuz yöneticiyi Admin Paneli ana sayfasına yolla.
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
