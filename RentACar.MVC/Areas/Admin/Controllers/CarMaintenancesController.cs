using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.Dtos.CarDtos;
using RentACar.Dtos.CarMaintenanceDtos;
using RentACar.MVC.Areas.Admin.Models.CarMaintenanceViewModels;
using RentACar.MVC.Controllers;
using RentACar.MVC.Models.Responses;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CarMaintenancesController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CarMaintenancesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/car-maintenances");
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
            var responseMessage = await client.DeleteAsync($"api/admin/car-maintenances/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            string errorMessage = await GetApiErrorMessageAsync(responseMessage);
            return Json(new { success = false, message = errorMessage });
        }

        [HttpGet]

        public async Task<IActionResult> Create(int? carId)
        {
            var viewModel = new CarMaintenanceCreateViewModel();
            if (carId.HasValue)
            {
                viewModel.CarMaintenanceCreate.CarId = carId.Value;
            }

            await PopulateDropdowns();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarMaintenanceCreateViewModel carMaintenanceCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(carMaintenanceCreateViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");
            var jsonData = JsonConvert.SerializeObject(carMaintenanceCreateViewModel.CarMaintenanceCreate);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/admin/car-maintenances", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            await HandleApiErrorAsync(responseMessage);
            await PopulateDropdowns();
            return View(carMaintenanceCreateViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/car-maintenances/{id}");
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
                    viewModel.CheckInTime = viewModel.CheckInTime.ToLocalTime();
                    if (viewModel.CheckOutTime.HasValue)
                    {
                        viewModel.CheckOutTime = viewModel.CheckOutTime.Value.ToLocalTime();
                    }
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
            var responseMessage = await client.PutAsync($"api/admin/car-maintenances/{carMaintenanceUpdateDto.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            await HandleApiErrorAsync(responseMessage);
            return View(carMaintenanceUpdateDto);
        }
        private async Task PopulateDropdowns()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.GetAsync("api/Cars");
            ViewBag.CarList = new List<object>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<CarResultDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var carList = responseBox.Data.Select(c => new
                    {
                        Id = c.Id,
                        DisplayName = $"{c.BrandName} {c.ModelName} ({c.Plate})"
                    }).ToList();
                    ViewBag.CarList = carList;
                }
            }
        }
    }
}
