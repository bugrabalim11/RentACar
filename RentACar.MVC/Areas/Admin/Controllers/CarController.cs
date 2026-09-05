using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.BrandDtos;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.ColorDtos;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "admin")]
    public class CarController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CarController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Cars");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<CarResponseDto>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<CarResultDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.DeleteAsync($"api/Cars/{id}");
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
        public async Task<IActionResult> Detail(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/Cars/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<CarDetailResponseDto>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CarCreateViewModel();
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var brandsResponseMessage = await client.GetAsync("api/Brands");
            var colorsResponseMessage = await client.GetAsync("api/Colors");
            if (brandsResponseMessage.IsSuccessStatusCode && colorsResponseMessage.IsSuccessStatusCode)
            {
                var brandsJsonData = await brandsResponseMessage.Content.ReadAsStringAsync();
                var colorsJsonData = await colorsResponseMessage.Content.ReadAsStringAsync();

                var brandsResponseBox = JsonConvert.DeserializeObject<BrandResponseDto>(brandsJsonData);
                var colorsResponseBox = JsonConvert.DeserializeObject<ColorResponseDto>(colorsJsonData);
                if (brandsResponseBox != null && brandsResponseBox.Data != null && colorsResponseBox != null && colorsResponseBox.Data != null)
                {
                    viewModel.Brands = brandsResponseBox.Data;
                    viewModel.Colors = colorsResponseBox.Data;
                }
            }
            return View(viewModel);
        }
    }
}
