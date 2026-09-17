using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Areas.Admin.Models.UserOperationClaimDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserOperationClaimController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserOperationClaimController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/UserOperationClaims/details");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<UserOperationClaimDetailDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<UserOperationClaimDetailDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.DeleteAsync($"api/UserOperationClaims/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Json(new { success = false, message = "Bu işlem için yetkiniz yok. Lütfen giriş yapın!" });
            }

            return Json(new { success = false, message = "API tarafında silme işlemi başarısız oldu!" });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new UserOperationClaimCreateViewModel();
            await PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        private async Task PopulateDropdowns(IUserOperationClaimDropdownsViewModel userOperationClaimDropdownsViewModel)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var usersResponseMessage = await client.GetAsync("api/Users");
            var claimsResponseMessage = await client.GetAsync("api/OperationClaims");
            if (usersResponseMessage.IsSuccessStatusCode && claimsResponseMessage.IsSuccessStatusCode)
            {
                var usersJsonData = await usersResponseMessage.Content.ReadAsStringAsync();
                var claimsJsonData = await claimsResponseMessage.Content.ReadAsStringAsync();

                var usersResponseBox = JsonConvert.DeserializeObject<ResponseModel<List<UserResultDto>>>(usersJsonData);
                var claimsResponseBox = JsonConvert.DeserializeObject<ResponseModel<List<OperationClaimResultDto>>>(claimsJsonData);
                if (usersResponseBox != null && usersResponseBox.Data != null && claimsResponseBox != null && claimsResponseBox.Data != null)
                {
                    userOperationClaimDropdownsViewModel.Users = usersResponseBox.Data;
                    userOperationClaimDropdownsViewModel.Roles = claimsResponseBox.Data;
                }
            }
        }
    }
}