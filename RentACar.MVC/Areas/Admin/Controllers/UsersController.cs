using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.OperationClaimDtos;
using RentACar.MVC.Areas.Admin.Models.UserDtos;
using RentACar.MVC.Models.Interfaces;
using RentACar.MVC.Models.Responses;
using System.Diagnostics.Eventing.Reader;
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
            // SENİOR NOTU: Sadece ID gönderdiğimiz için kuryenin (HttpClient) eline fiziki bir veri (Body) vermiyoruz, boş bir kutu ("") veriyoruz.
            // Ancak API'nin kapısındaki güvenlik çok katı olduğu için, kutu boş bile olsa üzerine
            // "Bu bir JSON paketidir" (application/json) etiketini yapıştırmak ZORUNDAYIZ. 
            // Aksi halde API kapıdan "Bu kutunun cinsi belli değil (415 Unsupported Media Type)" diyerek kargomuzu reddeder!
            var responseMessage = await client.PatchAsync($"api/Users/{id}/restore", new StringContent("", System.Text.Encoding.UTF8, "application/json"));
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
            var errorData = JsonConvert.DeserializeObject<ErrorDetailsDto>(errorJsonData);
            // TODO: Refactor: DRY prensibi gereği, bu hata yakalama if-else bloğu ileride BaseController'a taşınacak!
            if (errorData != null)
            {
                if (errorData.ValidationErrors != null && errorData.ValidationErrors.Any())
                {
                    foreach (var error in errorData.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorData.Message);
                }
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
            var errorData = JsonConvert.DeserializeObject<ErrorDetailsDto>(errorJsonData);
            // TODO: Refactor: DRY prensibi gereği, bu hata yakalama if-else bloğu ileride BaseController'a taşınacak!
            if (errorData != null)
            {
                if (errorData.ValidationErrors != null && errorData.ValidationErrors.Any())
                {
                    foreach (var error in errorData.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorData.Message);
                }
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
