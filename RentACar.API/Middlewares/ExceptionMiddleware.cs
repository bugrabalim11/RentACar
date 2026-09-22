using FluentValidation;
using RentACar.Core.Exceptions;
using System.Text.Json;

namespace RentACar.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Her şey yolundaysa isteğin bir sonraki aşamaya (Controller'a) geçmesini sağlar.
                await _next(context);
            }
            catch (Exception ex)
            {
                // Sistemde (Mutfakta/Manager'da) herhangi bir yerde hata fırlarsa buraya düşer!
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Döneceğimiz cevabın bir JSON formatı olduğunu belirtiyoruz
            context.Response.ContentType = "application/json";

            // Boş zarfı yarattık
            ErrorDetails errorDetails = new ErrorDetails(); 

            // 1. ODA: Validasyon (Kutuya 400 yaz ve listeyi doldur)
            if (exception is ValidationException validationException)
            {
                errorDetails.StatusCode = 400;
                errorDetails.Message = "Doğrulama kuralı ihlali!";
                errorDetails.ValidationErrors = validationException.Errors.Select(e => e.ErrorMessage);
            }
            // 2. ODA: Business (Kutuya 400 yaz ve mesajı BusinessException'dan al)
            else if (exception is BusinessException businessException)
            {
                errorDetails.StatusCode = 400;
                errorDetails.Message = businessException.Message;
                // ValidationErrors'a hiç dokunmuyoruz, o zaten boş/null kalacak
                // çünkü ValiadationRules'tan geçmiş BusinessRules'ta hata var.
            }
            // 3. ODA: Bilinmeyen Sistem Hataları (Kutuya 500 yaz)
            else
            {
                errorDetails.StatusCode = 500;
                errorDetails.Message = "Sistemde beklemeyen bir hata oluştu!";
            }

            // --- KODUN SON ÇIKIŞ NOKTASI (TEK KURYE) ---

            // Zarfın üstünde hangi kod yazıyorsa (400 mü 500 mü), asıl HTTP yanıtına onu veriyoruz:
            // Tarayıcıya hata kodunu veriyoruz yani
            context.Response.StatusCode = errorDetails.StatusCode;

            // Zarfı JSON'a çevir (Paketle)
            var result = JsonSerializer.Serialize(errorDetails);

            // Yolla gitsin!
            await context.Response.WriteAsync(result);
        }
    }
}
