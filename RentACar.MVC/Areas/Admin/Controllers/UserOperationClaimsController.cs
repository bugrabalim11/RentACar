using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.Dtos.OperationClaimDtos;
using RentACar.Dtos.UserDtos;
using RentACar.Dtos.UserOperationClaimDtos;
using RentACar.MVC.Areas.Admin.Models.UserOperationClaimViewModels;
using RentACar.MVC.Controllers;
using RentACar.MVC.Models.Responses;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UserOperationClaimsController : BaseController
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

            var responseMessage = await client.GetAsync("api/useroperationclaims/details");
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

            var responseMessage = await client.DeleteAsync($"api/admin/useroperationclaims/{id}");
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
            var viewModel = new UserOperationClaimCreateViewModel();
            await PopulateDropdowns();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserOperationClaimCreateViewModel userOperationClaimCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(userOperationClaimCreateViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(userOperationClaimCreateViewModel.UserOperationClaimCreate);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/admin/useroperationclaims", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            await HandleApiErrorAsync(responseMessage);
            await PopulateDropdowns();
            return View(userOperationClaimCreateViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/useroperationclaims/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<UserOperationClaimResultDto>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var viewModel = new UserOperationClaimUpdateViewModel
                    {
                        UserId = responseBox.Data.UserId,
                        UserOperationClaimUpdate = new UserOperationClaimUpdateDto
                        {
                            Id = responseBox.Data.Id,
                            OperationClaimId = responseBox.Data.OperationClaimId
                        }
                    };
                    await PopulateDropdowns();
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
                await PopulateDropdowns();
                return View(userOperationClaimUpdateViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(userOperationClaimUpdateViewModel.UserOperationClaimUpdate);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/admin/useroperationclaims/{userOperationClaimUpdateViewModel.UserOperationClaimUpdate.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            await HandleApiErrorAsync(responseMessage);
            await PopulateDropdowns();
            return View(userOperationClaimUpdateViewModel);
        }

        private async Task PopulateDropdowns()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var usersResponseMessage = await client.GetAsync("api/users");
            var claimsResponseMessage = await client.GetAsync("api/operationclaims");
            ViewBag.UserList = new List<object>();
            ViewBag.ClaimList = new List<object>();
            if (usersResponseMessage.IsSuccessStatusCode && claimsResponseMessage.IsSuccessStatusCode)
            {
                var usersJsonData = await usersResponseMessage.Content.ReadAsStringAsync();
                var claimsJsonData = await claimsResponseMessage.Content.ReadAsStringAsync();

                var usersResponseBox = JsonConvert.DeserializeObject<ResponseModel<List<UserResultDto>>>(usersJsonData);
                var claimsResponseBox = JsonConvert.DeserializeObject<ResponseModel<List<OperationClaimResultDto>>>(claimsJsonData);
                if (usersResponseBox != null && usersResponseBox.Data != null && claimsResponseBox != null && claimsResponseBox.Data != null)
                {
                    var userList = usersResponseBox.Data.Select(c => new
                    {
                        Id = c.Id,
                        UserDisplayName = $"{c.FirstName} {c.LastName} ({c.Email})"
                    }).ToList();
                    ViewBag.UserList = userList;

                    var claimList = claimsResponseBox.Data.Select(c => new
                    {
                        Id = c.Id,
                        ClaimName = c.Name
                    }).ToList();
                    ViewBag.ClaimList = claimList;
                }
            }
        }
    }
}