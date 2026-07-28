using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

                // 3. ADAMI KAPIDAN KOV: İşlemi burada kes ve adama 400 Bad Request (Geçersiz İstek) ile hataları fırlat.
                // İleride buraya kendi ErrorResult formatımızı da bağlayabiliriz.
                context.Result = new BadRequestObjectResult(errors);

                // 4. METOTTAN ÇIK: İşlemi kestiğimiz için metodu sonlandırıyoruz, adam içeri giremiyor.
                return;
            }

            // 5. ONAY: Eğer if bloğuna girmediyse (çanta temizse), adamı AVM'nin içine (bir sonraki adıma / Controller'a) yolla.
            await next();
        }
    }
}
