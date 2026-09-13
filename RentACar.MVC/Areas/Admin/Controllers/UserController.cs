using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Models.Responses;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Users");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<UserResultForAdminDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<UserResultForAdminDto>());
        }
    }
}
