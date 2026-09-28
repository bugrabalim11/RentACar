using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Controllers;
using RentACar.MVC.Models.Responses;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class OperationClaimsController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OperationClaimsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.GetAsync("api/OperationClaims");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<OperationClaimResultDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<OperationClaimResultDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.DeleteAsync($"api/OperationClaims/{id}");
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
            var viewModel = new OperationClaimCreateDto();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OperationClaimCreateDto operationClaimCreateDto)
        {
            if (!ModelState.IsValid) { return View(operationClaimCreateDto); }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(operationClaimCreateDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/OperationClaims", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            await HandleApiErrorAsync(responseMessage);
            return View(operationClaimCreateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/OperationClaims/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<OperationClaimResultDto>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var viewModel = new OperationClaimUpdateDto
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
        public async Task<IActionResult> Update(OperationClaimUpdateDto operationClaimUpdateDto)
        {
            if (!ModelState.IsValid) { return View(operationClaimUpdateDto); }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(operationClaimUpdateDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/OperationClaims/{operationClaimUpdateDto.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            await HandleApiErrorAsync(responseMessage);
            return View(operationClaimUpdateDto);
        }
    }
}