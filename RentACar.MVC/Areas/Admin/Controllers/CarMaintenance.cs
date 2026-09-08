using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.CarMaintenanceDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;

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
