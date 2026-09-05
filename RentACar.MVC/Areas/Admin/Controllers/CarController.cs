using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Areas.Admin.Models.BrandDtos;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.ColorDtos;
using RentACar.MVC.Areas.Admin.Models.ErrorResponseDtos;
using System.Text;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "admin")]
    public class CarController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CarController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync("api/Cars");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<CarResponseDto>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return View(new List<CarResultDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var responseMessage = await client.DeleteAsync($"api/Cars/{id}");
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

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/Cars/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<CarDetailResponseDto>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    return View(responseBox.Data);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CarCreateViewModel();
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var brandsResponseMessage = await client.GetAsync("api/Brands");
            var colorsResponseMessage = await client.GetAsync("api/Colors");
            if (brandsResponseMessage.IsSuccessStatusCode && colorsResponseMessage.IsSuccessStatusCode)
            {
                var brandsJsonData = await brandsResponseMessage.Content.ReadAsStringAsync();
                var colorsJsonData = await colorsResponseMessage.Content.ReadAsStringAsync();

                var brandsResponseBox = JsonConvert.DeserializeObject<BrandResponseDto>(brandsJsonData);
                var colorsResponseBox = JsonConvert.DeserializeObject<ColorResponseDto>(colorsJsonData);
                if (brandsResponseBox != null && brandsResponseBox.Data != null && colorsResponseBox != null && colorsResponseBox.Data != null)
                {
                    viewModel.Brands = brandsResponseBox.Data;
                    viewModel.Colors = colorsResponseBox.Data;
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarCreateViewModel carCreateViewModel)
        {
            // GÜVENLİK KONTROLÜ (Form eksik mi?)
            if (!ModelState.IsValid)
            {
                // Form eksikse, müşteriye geri yollayacağımız tepsinin boşalan listelerini Komi'ye doldurtuyoruz.
                await PopulateDropdowns(carCreateViewModel);
                return View(carCreateViewModel);
            }

            var newClient = _httpClientFactory.CreateClient("RentACarApi");
            var jsonData = JsonConvert.SerializeObject(carCreateViewModel.CarCreate);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await newClient.PostAsync("api/Cars", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                // Hata sayfası döneceği için tepsiyi yine doldurmalıyız!
                await PopulateDropdowns(carCreateViewModel);
                return View(carCreateViewModel);
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorResponseDto>(errorJsonData);
            if (errorData != null)
            {
                ModelState.AddModelError(string.Empty, errorData.Message);
            }

            // EN ALTTA BİR DAHA DOLDUR!
            // Buraya kadar geldiysek kesin bir hata vardır ve sayfa geri dönecektir. Listeleri doldurmadan yollama!
            await PopulateDropdowns(carCreateViewModel);
            return View(carCreateViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");

            var responseMessage = await client.GetAsync($"api/Cars/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var responseBox = JsonConvert.DeserializeObject<GetByIdCarResponseDto>(jsonData);
                if (responseBox != null && responseBox.Data != null)
                {
                    var viewModel = new CarUpdateViewModel
                    {
                        CarUpdate = responseBox.Data
                    };
                    await PopulateDropdowns(viewModel);
                    return View(viewModel);
                }
            }
            return RedirectToAction("Index");
        }

        private async Task PopulateDropdowns(CarCreateViewModel carCreateViewModel)
        {
            // SENİOR NOTU (YARDIMCI METOT):
            // Bu metodun TEK BİR GÖREVİ vardır: Parametre olarak gelen tepsinin (model) içine
            // API'den güncel Marka ve Renk listelerini çekip yerleştirmek.
            // Başka hiçbir işe (Validasyon veya View döndürme) karışmaz!

            var client = _httpClientFactory.CreateClient("RentACarApi");
            var brandsResponseMessage = await client.GetAsync("api/Brands");
            var colorsResponseMessage = await client.GetAsync("api/Colors");
            if (brandsResponseMessage.IsSuccessStatusCode && colorsResponseMessage.IsSuccessStatusCode)
            {
                var brandsJsonData = await brandsResponseMessage.Content.ReadAsStringAsync();
                var colorsJsonData = await colorsResponseMessage.Content.ReadAsStringAsync();

                var brandsResponseBox = JsonConvert.DeserializeObject<BrandResponseDto>(brandsJsonData);
                var colorsResponseBox = JsonConvert.DeserializeObject<ColorResponseDto>(colorsJsonData);
                if (brandsResponseBox != null && brandsResponseBox.Data != null && colorsResponseBox != null && colorsResponseBox.Data != null)
                {
                    // DİKKAT: 'new ViewModel()' DEMİYORUZ! Kullanıcının doldurduğu mevcut 'viewModel' içine 
                    // sadece eksik olan listeleri monte ediyoruz ki adamın yazdığı veriler silinmesin!
                    carCreateViewModel.Brands = brandsResponseBox.Data;
                    carCreateViewModel.Colors = colorsResponseBox.Data;
                }
            }
        }

        private async Task PopulateDropdowns(CarUpdateViewModel carUpdateViewModel)
        {
            var client = _httpClientFactory.CreateClient("RentACarApi");
            var brandsResponseMessage = await client.GetAsync("api/Brands");
            var colorsResponseMessage = await client.GetAsync("api/Colors");
            if (brandsResponseMessage.IsSuccessStatusCode && colorsResponseMessage.IsSuccessStatusCode)
            {
                var brandsJsonData = await brandsResponseMessage.Content.ReadAsStringAsync();
                var colorsJsonData = await colorsResponseMessage.Content.ReadAsStringAsync();

                var brandsResponseBox = JsonConvert.DeserializeObject<BrandResponseDto>(brandsJsonData);
                var colorsResponseBox = JsonConvert.DeserializeObject<ColorResponseDto>(colorsJsonData);
                if (brandsResponseBox != null && brandsResponseBox.Data != null && colorsResponseBox != null && colorsResponseBox.Data != null)
                {
                    carUpdateViewModel.Brands = brandsResponseBox.Data;
                    carUpdateViewModel.Colors = colorsResponseBox.Data;
                }
            }
        }
    }
}
