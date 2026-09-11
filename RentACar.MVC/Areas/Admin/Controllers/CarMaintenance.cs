using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.CarMaintenanceDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarMaintenance : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CarMaintenance(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/CarMaintenances");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<CarMaintenanceResultDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<CarMaintenanceResultDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.DeleteAsync($"api/CarMaintenances/{id}");
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
        // Ana menüden "Yeni Bakım Ekle"ye basılırsa kurye not getirmez(carId null olur).
        // Arabalar listesinden basılırsa kurye not getirir(carId dolu olur).
        public async Task<IActionResult> Create(int? carId)
        {
            // 1. KURYEDEN GELEN KUTU: Kutu boş (null) da olabilir, içinde bir sayı da olabilir.
            // Garsonun tepsisini (ViewModel) 'new' diyerek fiziksel olarak inşa ediyoruz (Constructor burada devreye girip içindeki DTO'yu da üretiyor).
            var viewModel = new CarMaintenanceCreateViewModel();

            // 2. VIP KONTROLÜ: Eğer kurye bir araba ID'si getirdiyse (Kısayol butonuna basıldıysa)
            if (carId.HasValue)
            {
                // Kuryenin kutusundaki gerçek sayıyı (.Value) al ve DTO'nun içine yerleştir.
                viewModel.CarMaintenanceCreate.CarId = carId.Value;
            }

            // 3. STANDART İŞLEM: Adam VIP (carId var) olsa da olmasa da, o vitrindeki Dropdown (Araçlar Listesi) dolmak ZORUNDA!
            await PopulateDropdowns(viewModel);

            // 4. SERVİS: Hazırlanan tepsiyi müşteriye (View'a) sun.
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarMaintenanceCreateViewModel carMaintenanceCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(carMaintenanceCreateViewModel);
                return View(carMaintenanceCreateViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");
            var jsonData = JsonConvert.SerializeObject(carMaintenanceCreateViewModel.CarMaintenanceCreate);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/CarMaintenances", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                await PopulateDropdowns(carMaintenanceCreateViewModel);
                return View(carMaintenanceCreateViewModel);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            // Kuryeden nesne (Dto) değil, direkt metin (string) listesi geldiğini anladık.
            var errorData = JsonConvert.DeserializeObject<List<string>>(errorJsonData);

            if (errorData != null)
            {
                foreach (var message in errorData) // Sepetteki (errorData) her bir Post-it kağıdını (message) al
                {
                    // Vitrine direkt o düz metni bas
                    ModelState.AddModelError(string.Empty, message);
                }
            }

            await PopulateDropdowns(carMaintenanceCreateViewModel);
            return View(carMaintenanceCreateViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/CarMaintenances/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<CarMaintenanceResultDto>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var viewModel = new CarMaintenanceUpdateDto
                    {
                        Id = responseBox.Data.Id,
                        Description = responseBox.Data.Description,
                        CheckInTime = responseBox.Data.CheckInTime,
                        CheckOutTime = responseBox.Data.CheckOutTime
                    };
                    return View(viewModel);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(CarMaintenanceUpdateDto carMaintenanceUpdateDto)
        {
            if (!ModelState.IsValid) { return View(carMaintenanceUpdateDto); }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(carMaintenanceUpdateDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/CarMaintenances/{carMaintenanceUpdateDto.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                return View(carMaintenanceUpdateDto);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<List<string>>(errorJsonData);
            if (errorData != null)
            {
                foreach (var message in errorData)
                {
                    ModelState.AddModelError(string.Empty, message);
                }
            }
            return View(carMaintenanceUpdateDto);
        }
        private async Task PopulateDropdowns(ICarMaintenanceDropdownViewModel carMaintenanceDropdownViewModel)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.GetAsync("api/Cars");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<CarResultDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    carMaintenanceDropdownViewModel.Cars = responseBox.Data;
                }
            }
        }
    }
}
