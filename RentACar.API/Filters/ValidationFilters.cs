using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentACar.Core.Exceptions;

namespace RentACar.API.Filters
{
    public class ValidationFilters : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. KONTROL: Adamın çantası (ModelState) kurallara UYMUYORSA (İçinde bıçak/hata varsa)
            if (!context.ModelState.IsValid)
            {
                // 2. HATALARI TOPLA: Çantanın tüm ceplerini (Values) gez, içindeki hataları (Errors)
                // bul ve sadece hata mesajlarını (ErrorMessage) bir listeye çevir.
                var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                // 3. ADAMI KAPIDAN KOV (AMA BİZİM RESMİ ŞİRKET KUTUMUZLA!)
                // Burada, API'nin Middleware tarafında kullandığın 'ErrorDetails' sınıfından bir kutu yaratıyoruz.
                var erroDetails = new ErrorDetails
                {
                    StatusCode = 400,
                    Message = "Doğrulama kuralı ihlali! Lütfen girdiğiniz bilgileri kontrol edin.",
                    ValidationErrors = errors // Güvenliğin bulduğu o çıplak listeyi, kutunun içine özenle yerleştiriyoruz!
                };

                // 4. Adamın kafasına çıplak listeyi değil, bu jilet gibi hazırlanmış kutuyu fırlatıyoruz!
                context.Result = new BadRequestObjectResult(erroDetails);

                return;
            }

            // 5. ONAY: Eğer if bloğuna girmediyse (çanta temizse), adamı AVM'nin içine (bir sonraki adıma / Controller'a) yolla.
            await next();
        }
    }
}
