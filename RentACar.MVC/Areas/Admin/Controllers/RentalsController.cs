using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.RentalDtos;
using RentACar.MVC.Models.Responses;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class RentalsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RentalsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.GetAsync("api/Rentals");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<RentalResultDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<RentalResultDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.DeleteAsync($"api/Rentals/{id}");
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
    }
}