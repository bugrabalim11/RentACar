using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.Dtos.CarDtos;
using RentACar.Dtos.CustomerDtos;
using RentACar.Dtos.OfficeDtos;
using RentACar.Dtos.RentalDtos;
using RentACar.MVC.Areas.Admin.Models.RentalViewModels;
using RentACar.MVC.Controllers;
using RentACar.MVC.Models.Responses;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class RentalsController : BaseController
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
            var responseMessage = await client.GetAsync("api/admin/Rentals");
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
            var responseMessage = await client.DeleteAsync($"api/admin/Rentals/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            string errorMessage = await GetApiErrorMessageAsync(responseMessage);
            return Json(new { success = false, message = errorMessage });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new RentalCreateByAdminViewModel();
            viewModel.RentalCreateByAdmin = new RentalCreateByAdminDto();
            await PopulateDropdowns();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RentalCreateByAdminViewModel rentalCreateByAdminViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(rentalCreateByAdminViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(rentalCreateByAdminViewModel.RentalCreateByAdmin);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/admin/Rentals", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            await HandleApiErrorAsync(responseMessage);
            await PopulateDropdowns();
            return View(rentalCreateByAdminViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/admin/Rentals/{id}");
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
                    await PopulateDropdowns();

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
                await PopulateDropdowns();
                return View(viewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(viewModel.RentalUpdateByAdmin);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/admin/Rentals/{viewModel.RentalUpdateByAdmin.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            await HandleApiErrorAsync(responseMessage);
            await PopulateDropdowns();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/admin/Rentals/{id}");
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

        private async Task PopulateDropdowns()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var customerResponseMessage = await client.GetAsync("api/Customers");
            var carResponseMessage = await client.GetAsync("api/Cars");
            var officeResponseMessage = await client.GetAsync("api/Offices");
            ViewBag.CustomerList = new List<object>();
            ViewBag.CarList = new List<object>(); 
            ViewBag.OfficeList = new List<object>();
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
                    var customerList = customerResponseBox.Data.Select(c => new
                    {
                        Id = c.Id,
                        CustomerDisplayName = $"{c.FirstName} {c.LastName} - TC: {c.NationalIdentity}"
                    }).ToList();
                    ViewBag.CustomerList = customerList;

                    var carList = carResponseBox.Data.Select(c => new
                    {
                        Id = c.Id,
                        CarDisplayName = $"{c.BrandName} {c.ModelName} ({c.Plate})"
                    }).ToList();
                    ViewBag.CarList = carList;

                    var officeList = officeResponseBox.Data.Select(o => new
                    {
                        Id = o.Id,
                        OfficeName = o.Name
                    }).ToList();
                    ViewBag.OfficeList = officeList;
                }
            }
        }
    }
}