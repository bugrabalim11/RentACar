using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.ErrorResponseDtos;
using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Users/admin-details");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<UserResultByAdminDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<UserResultByAdminDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.DeleteAsync(($"api/Users/{id}"));
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Json(new { success = false, message = "Bu işlem için yetkiniz yok. Lütfen giriş yapın!" });
            }
            return Json(new { success = false, message = "Api tarafından silme işlemi başarısız oldu!" });
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            // NOT: Patch işlemi kural gereği bir veri paketi (Body) bekler.
            // Sadece ID ile işlem yaptığımız ve ekstra verimiz olmadığı için kuryenin eline boş bir kutu (StringContent) veriyoruz.
            var responseMessage = await client.PatchAsync($"api/Users/{id}/restore", new StringContent(""));
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Json(new { success = false, message = "Bu işlem için yetkiniz yok. Lütfen giriş yapın!" });
            }
            return Json(new { success = false, message = "Api tarafından geri getirme işlemi başarısız oldu!" });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new UserCreateByAdminViewModel();
            // SENİOR NOTU: Tepsinin (ViewModel) içine boş bir UserCreateForAdminDto (Sipariş Fişi) koyuyoruz.
            // Eğer bunu yapmazsak, View (HTML) tarafı '@Model.UserCreate.FirstName' gibi değerleri okumaya çalıştığında 
            // "Masa var ama üstünde kağıt yok!" diyerek Null Reference Exception (CS0120) hatası fırlatabilir.
            // Bu hamle, bellekte (RAM) o boş kağıda fiziksel bir yer ayırır. İşimi şansa bırakmıyoruz!
            viewModel.UserCreate = new UserCreateByAdminDto();
            await PopulateDropdown(viewModel);
            return View(viewModel);
        }

        // TODO TESTİ TEKRAR YAP
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateByAdminViewModel userCreateForAdminViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdown(userCreateForAdminViewModel);
                return View(userCreateForAdminViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(userCreateForAdminViewModel.UserCreate);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/Users", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                await PopulateDropdown(userCreateForAdminViewModel);
                return View(userCreateForAdminViewModel);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);
            if (errorData != null)
            {
                ModelState.AddModelError(string.Empty, errorData.Message);
            }

            await PopulateDropdown(userCreateForAdminViewModel);
            return View(userCreateForAdminViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/Users/{id}/update-form");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<UserUpdateByAdminDto>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var viewModel = new UserUpdateByAdminViewModel
                    {
                        UserUpdate = responseBox.Data
                    };
                    await PopulateDropdown(viewModel);
                    return View(viewModel);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateByAdminViewModel userUpdateForAdminViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdown(userUpdateForAdminViewModel);
                return View(userUpdateForAdminViewModel);
            }

            var client = _httpClientFactory.CreateClient("RentACarApi");

            var jsonData = JsonConvert.SerializeObject(userUpdateForAdminViewModel.UserUpdate);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"api/Users/{userUpdateForAdminViewModel.UserUpdate.Id}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                await PopulateDropdown(userUpdateForAdminViewModel);
                return View(userUpdateForAdminViewModel);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);
            if (errorData != null)
            {
                ModelState.AddModelError(string.Empty, errorData.Message);
            }
            await PopulateDropdown(userUpdateForAdminViewModel);
            return View(userUpdateForAdminViewModel);
        }

        private async Task PopulateDropdown(IUserDropdownViewModel userDropdownViewModel)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.GetAsync("api/OperationClaims");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<ResponseModel<List<OperationClaimResultDto>>>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    userDropdownViewModel.Roles = responseBox.Data;
                }
            }
        }
    }
}
