using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.CustomerDtos;
using RentACar.MVC.Areas.Admin.Models.OfficeDtos;
using RentACar.MVC.Areas.Admin.Models.RentalDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;
using System.Text;

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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new RentalCreateByAdminViewModel();
            viewModel.RentalCreateByAdmin = new RentalCreateByAdminDto();
            await PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RentalCreateByAdminViewModel rentalCreateByAdminViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(rentalCreateByAdminViewModel);
                return View(rentalCreateByAdminViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(rentalCreateByAdminViewModel.RentalCreateByAdmin);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/Rentals", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                await PopulateDropdowns(rentalCreateByAdminViewModel);
                return View(rentalCreateByAdminViewModel);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorDetailsDto>(errorJsonData);
            if (errorData != null)
            {
                if (errorData.ValidationErrors != null && errorData.ValidationErrors.Any())
                {
                    foreach (var error in errorData.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorData.Message);
                }
            }

            await PopulateDropdowns(rentalCreateByAdminViewModel);
            return View(rentalCreateByAdminViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/Rentals/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<RentalUpdateByAdminDto>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var viewModel = new RentalUpdateByAdminViewModel
                    {
                        RentalUpdateByAdmin = responseBox.Data
                    };
                    await PopulateDropdowns(viewModel);

                    // Örnek Pusu Kurulumu:
                    // View da yaptığımız Türkiye saat dilimine çevirme işlemini burada da getiriyoruz
                    viewModel.RentalUpdateByAdmin.RentDate = viewModel.RentalUpdateByAdmin.RentDate.ToLocalTime();
                    if (viewModel.RentalUpdateByAdmin.ReturnDate.HasValue)
                    {
                        viewModel.RentalUpdateByAdmin.ReturnDate = viewModel.RentalUpdateByAdmin.ReturnDate.Value.ToLocalTime();
                    }

                    return View(viewModel);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(RentalUpdateByAdminViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(viewModel.RentalUpdateByAdmin);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/Rentals/{viewModel.RentalUpdateByAdmin.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorDetailsDto>(errorJsonData);
            if (errorData != null)
            {
                if (errorData.ValidationErrors != null && errorData.ValidationErrors.Any())
                {
                    foreach (var error in errorData.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorData.Message);
                }
            }

            await PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/Rentals/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<RentalDetailDto>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return RedirectToAction("Index");
        }

        private async Task PopulateDropdowns(IRentalDropdownsViewModel dropdownsViewModel)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var customerResponseMessage = await client.GetAsync("api/Customers");
            var carResponseMessage = await client.GetAsync("api/Cars");
            var officeResponseMessage = await client.GetAsync("api/Offices");
            if (customerResponseMessage.IsSuccessStatusCode && carResponseMessage.IsSuccessStatusCode && officeResponseMessage.IsSuccessStatusCode)
            {
                var customerJsonData = await customerResponseMessage.Content.ReadAsStringAsync();
                var carJsonData = await carResponseMessage.Content.ReadAsStringAsync();
                var officeJsonData = await officeResponseMessage.Content.ReadAsStringAsync();

                var customerResponseBox = JsonConvert.DeserializeObject<ResponseModel<List<CustomerResultDto>>>(customerJsonData);
                var carResponseBox = JsonConvert.DeserializeObject<ResponseModel<List<CarResultDto>>>(carJsonData);
                var officeResponseBox = JsonConvert.DeserializeObject<ResponseModel<List<OfficeResultDto>>>(officeJsonData);
                if (customerResponseBox != null && customerResponseBox.Data != null && carResponseBox != null && carResponseBox.Data != null && officeResponseBox != null && officeResponseBox.Data != null)
                {
                    dropdownsViewModel.Customers = customerResponseBox.Data;
                    dropdownsViewModel.Cars = carResponseBox.Data;
                    dropdownsViewModel.Offices = officeResponseBox.Data;
                }
            }
        }
    }
}