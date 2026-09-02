using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.ErrorResponseDtos;
using RentACar.MVC.Areas.Admin.Models.OfficeDtos;
using System.Text;

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
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<OfficeResultDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.DeleteAsync($"api/Offices/{id}");
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OfficeCreateDto officeCreateDto)
        {
            if (!ModelState.IsValid) { return View(officeCreateDto); }
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(officeCreateDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/Offices", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                return View(officeCreateDto);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);
            if (errorData != null)
            {
                ModelState.AddModelError(string.Empty, errorData.Message);
            }
            return View(officeCreateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var resposneMessage = await client.GetAsync($"api/Offices/{id}");
            if (resposneMessage.IsSuccessStatusCode)
            {
                var jsonData = await resposneMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<GetByIdOfficeResponseDto>(jsonData);
                if (response != null && response.Data != null)
                {
                    return View(response.Data);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(OfficeUpdateDto officeUpdateDto)
        {
            if (!ModelState.IsValid) { return View(officeUpdateDto); }
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(officeUpdateDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/Offices/{officeUpdateDto.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                return View(officeUpdateDto);
            }
            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);
            if (errorData != null)
            {
                ModelState.AddModelError(string.Empty, errorData.Message);
            }
            return View(officeUpdateDto);
        }
    }
}
