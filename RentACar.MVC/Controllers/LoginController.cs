using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.Dtos.AuthDtos;
using RentACar.MVC.Extensions;

namespace RentACar.MVC.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserForLoginDto userForLoginDto)
        {
            if (!ModelState.IsValid) { return View(userForLoginDto); }
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(userForLoginDto);
            var stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/auths/login", stringContent);
            if (!responseMessage.IsSuccessStatusCode)
            {
                await HandleApiErrorAsync(responseMessage);
                return View(userForLoginDto);
            }

            var successJsonData = await responseMessage.Content.ReadAsStringAsync();

            var tokenData = JsonConvert.DeserializeObject<AccessTokenResponseDto>(successJsonData);
            if (tokenData == null)
            {
                ModelState.AddModelError(string.Empty, "Token bilgisi alınamadı.");
                return View(userForLoginDto);
            }

            Response.Cookies.Append("AccessToken", tokenData.Token, new CookieOptions
            {
                HttpOnly = true, 
                SameSite = SameSiteMode.Strict,
                Secure = false,
                Expires = DateTime.Now.AddDays(1) 
            });

            var userRole = tokenData.Token.GetRoleFromJwt();
            if (userRole == "admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
