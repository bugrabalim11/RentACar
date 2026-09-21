using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.ErrorResponseDtos;
using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Areas.Admin.Models.UserOperationClaimDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserOperationClaimsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserOperationClaimsController(IHttpClientFactory httpClientFactory)
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

        [HttpPost]
        public async Task<IActionResult> Create(UserOperationClaimCreateViewModel userOperationClaimCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(userOperationClaimCreateViewModel);
                return View(userOperationClaimCreateViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(userOperationClaimCreateViewModel.UserOperationClaimCreate);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/UserOperationClaims", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                await PopulateDropdowns(userOperationClaimCreateViewModel);
                return View(userOperationClaimCreateViewModel);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);
            if (errorData != null)
            {
                ModelState.AddModelError(string.Empty, errorData.Message);
            }

            await PopulateDropdowns(userOperationClaimCreateViewModel);
            return View(userOperationClaimCreateViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/UserOperationClaims/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<UserOperationClaimUpdateDto>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var viewModel = new UserOperationClaimUpdateViewModel
                    {
                        UserOperationClaimUpdate = responseBox.Data
                    };
                    await PopulateDropdowns(viewModel);
                    return View(viewModel);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserOperationClaimUpdateViewModel userOperationClaimUpdateViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(userOperationClaimUpdateViewModel);
                return View(userOperationClaimUpdateViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(userOperationClaimUpdateViewModel.UserOperationClaimUpdate);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/UserOperationClaims/{userOperationClaimUpdateViewModel.UserOperationClaimUpdate.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                await PopulateDropdowns(userOperationClaimUpdateViewModel);
                return View(userOperationClaimUpdateViewModel);
            }

            var errorjsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorjsonData);
            if(errorData != null)
            {
                ModelState.AddModelError(string.Empty, errorData.Message);
            }

            await PopulateDropdowns(userOperationClaimUpdateViewModel);
            return View(userOperationClaimUpdateViewModel);
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