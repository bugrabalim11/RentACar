using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Models.Responses;

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
    }
}