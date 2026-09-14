using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Models.Responses;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Users/getallforadmin");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<UserResultForAdminDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<UserResultForAdminDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.DeleteAsync(($"api/Users/{id}"));
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

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            // NOT: Patch işlemi kural gereği bir veri paketi (Body) bekler.
            // Sadece ID ile işlem yaptığımız ve ekstra verimiz olmadığı için kuryenin eline boş bir kutu (StringContent) veriyoruz.
            var responseMessage = await client.PatchAsync($"api/Users/restore/{id}", new StringContent(""));
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Json(new { success = false, message = "Bu işlem için yetkiniz yok. Lütfen giriş yapın!" });
            }
            return Json(new { success = false, message = "Api tarafından geri getirme işlemi başarısız oldu!" });
        }
    }
}
