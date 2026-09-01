using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.OfficeDtos;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OfficeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OfficeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Offices");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<OfficeResponseDto>(jsonData);
                if (responseBox != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View();
        }
    }
}
