using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.ErrorResponseDtos;
using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Models.Responses;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OperationClaimController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OperationClaimController(IHttpClientFactory httpClientFactory)
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

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                return View(operationClaimCreateDto);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);
            if (errorData != null)
            {
                ModelState.AddModelError(string.Empty, errorData.Message);
            }

            return View(operationClaimCreateDto);
        }
    }
}