using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.CustomerDtos;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Controllers;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CustomersController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Customers");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<CustomerResultDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<CustomerResultDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.DeleteAsync($"api/Customers/{id}");
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
            var viewModel = new CustomerCreateByAdminViewModel();
            await PopulateDropdown(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerCreateByAdminViewModel customerCreateByAdminViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdown(customerCreateByAdminViewModel);
                return View(customerCreateByAdminViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(customerCreateByAdminViewModel.CustomerCreateByAdmin);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/Customers", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            await HandleApiErrorAsync(responseMessage);
            await PopulateDropdown(customerCreateByAdminViewModel);
            return View(customerCreateByAdminViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/Customers/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<CustomerUpdateByAdminDto>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var viewModel = new CustomerUpdateByAdminViewModel
                    {
                        CustomerUpdateByAdmin = responseBox.Data
                    };
                    await PopulateDropdown(viewModel);
                    return View(viewModel);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(CustomerUpdateByAdminViewModel customerUpdateByAdminViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdown(customerUpdateByAdminViewModel);
                return View(customerUpdateByAdminViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(customerUpdateByAdminViewModel.CustomerUpdateByAdmin);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/Customers/{customerUpdateByAdminViewModel.CustomerUpdateByAdmin.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            await HandleApiErrorAsync(responseMessage);
            await PopulateDropdown(customerUpdateByAdminViewModel);
            return View(customerUpdateByAdminViewModel);
        }

        private async Task PopulateDropdown(ICustomerDropdownViewModel dropdownViewModel)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Users");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<UserResultDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    dropdownViewModel.Users = responseBox.Data;
                }
            }
        }
    }
}
