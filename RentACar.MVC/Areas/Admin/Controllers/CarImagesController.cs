using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.Dtos.CarImageDtos;
using RentACar.MVC.Controllers;
using RentACar.MVC.Models.Responses;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CarImagesController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CarImagesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int carId)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/carimages/cars/{carId}");
            ViewBag.CarId = carId;
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<CarImageDetailDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<CarImageDetailDto>());
        }

        [HttpGet]
        public async Task<IActionResult> DeletedImages(int carId)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.GetAsync($"api/admin/carimages/cars/{carId}/deleted");
            ViewBag.CarId = carId;
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<CarImageDeletedDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<CarImageDeletedDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.DeleteAsync($"api/admin/carimages/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            string errorMessage = await GetApiErrorMessageAsync(responseMessage);
            return Json(new { success = false, message = errorMessage });
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.PatchAsync($"api/admin/carimages/{id}/restore", null);
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            string errorMessage = await GetApiErrorMessageAsync(responseMessage);
            return Json(new { success = false, message = errorMessage });
        }

        [HttpGet]
        public IActionResult Create(int carId)
        {
            CarImageCreateDto carImageCreateDto = new CarImageCreateDto
            {
                CarId = carId,
            };
            return View(carImageCreateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarImageCreateDto carImageCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(carImageCreateDto);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(carImageCreateDto.CarId.ToString()), "CarId");
            var streamContent = new StreamContent(carImageCreateDto.ImageFile.OpenReadStream());
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(carImageCreateDto.ImageFile.ContentType);
            content.Add(streamContent, "ImageFile", carImageCreateDto.ImageFile.FileName);

            var responseMessage = await client.PostAsync("api/admin/carimages", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { carId = carImageCreateDto.CarId });
            }

            await HandleApiErrorAsync(responseMessage);
            return View(carImageCreateDto);
        }
    }
}

