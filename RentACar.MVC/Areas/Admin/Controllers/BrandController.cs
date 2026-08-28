using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.BrandDtos;

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
                var values = JsonConvert.DeserializeObject<List<ResultBrandDto>>(jsonData);
                return View(values);
            }
            // API çağrısı başarısız olursa, sayfa patlamasın diye boş dönüyoruz.
            return View();
        }
    }
}
