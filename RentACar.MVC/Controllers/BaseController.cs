using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Models.Responses;

namespace RentACar.MVC.Controllers
{
    public class BaseController : Controller
    {
        // SENİOR NOTU 1: MAĞAZA TAHTASI (ModelState) ÇEVİRMENİ
        // Bu metot geleneksel Form gönderimlerinde (Create/Update) kullanılır. 
        // API'den dönen hatayı okur ve doğrudan sayfanın üzerindeki hata tahtasına (ModelState) kırmızı yazıyla yazar.
        // "protected" demek: Sadece benden miras alan şantiyeler bu aleti kullanabilir.
        protected async Task HandleApiErrorAsync(HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Bu işlem için yetkiniz yok. Lütfen giriş yapın!");
                return;
            }
            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorDetailsDto>(errorJsonData);
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
        }

        // SENİOR NOTU 2: TELEFON KURYESI (Fetch/AJAX) ÇEVİRMENİ
        // Bu metot arka planda sessizce çalışan JS (Delete/Restore vb.) isteklerinde kullanılır.
        // Hataları ModelState'e YAZMAZ, çünkü ortada yeniden yüklenen bir sayfa yoktur! 
        // Sadece hatayı metin (string) olarak döner ki JS tarafındaki SweetAlert/Toastr bu metni ekranda gösterebilsin.
        protected async Task<string> GetApiErrorMessageAsync(HttpResponseMessage responseMessage)
        {
            // 1. Kalkan: 401 ise direkt mesajı dön
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return "Bu işlem için yetkiniz yok. Lütfen giriş yapın!";
            }

            // 2. Kutu Açılışı: 400 Bad Request vb. ise JSON'ı aç
            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorDetailsDto>(errorJsonData);
            if (errorData != null)
            {
                // Kapı memuru (FluentValidation) liste fırlattıysa, aralarına boşluk koyarak tek cümle yap
                if(errorData.ValidationErrors!=null && errorData.ValidationErrors.Any())
                {
                    return string.Join(" ", errorData.ValidationErrors);
                }

                // İş asistanı (BusinessRules) tek mesaj fırlattıysa
                return errorData.Message;
            }

            return "Bilinmeyen bir hata oluştu!";
        }
    }
}