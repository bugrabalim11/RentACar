using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.CarMaintenanceDtos;
using RentACar.MVC.Areas.Admin.Models.ErrorResponseDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;
using System.Text;

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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CarMaintenanceCreateViewModel();
            await PopulateDropdowns(viewModel);
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
