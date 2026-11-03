using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.Dtos.BrandDtos;
using RentACar.MVC.Controllers;
using RentACar.MVC.Models.Responses;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class BrandsController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BrandsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Brands");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<BrandResultDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<BrandResultDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.DeleteAsync($"api/admin/brands/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            string errorMessage = await GetApiErrorMessageAsync(responseMessage);
            return Json(new { success = false, message = errorMessage });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new BrandCreateDto();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateDto brandCreateDto)
        {
            if (!ModelState.IsValid) { return View(brandCreateDto); }
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var jsonData = JsonConvert.SerializeObject(brandCreateDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/admin/brands", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            await HandleApiErrorAsync(responseMessage);
            return View(brandCreateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessasge = await client.GetAsync($"api/Brands/{id}");
            if (responseMessasge.IsSuccessStatusCode)
            {
                var jsonData = await responseMessasge.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<BrandResultDto>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var viewModel = new BrandUpdateDto
                    {
                        Id = responseBox.Data.Id, 
                        Name = responseBox.Data.Name
                    };
                    return View(viewModel);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(BrandUpdateDto brandUpdateDto)
        {
            if (!ModelState.IsValid) { return View(brandUpdateDto); }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(brandUpdateDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/admin/brands/{brandUpdateDto.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            await HandleApiErrorAsync(responseMessage);
            return View(brandUpdateDto);
        }
    }
}
