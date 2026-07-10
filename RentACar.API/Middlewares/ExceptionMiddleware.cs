using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System.Net;
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

            // Eğer yakaladığımız hata FluentValidation'ın fırlattığı kuralsa
            if (exception is ValidationException validationException)
            {
                // Durum kodunu 400 Bad Request (Hatalı İstek) yapıyoruz
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                // Sadece senin o yazdığın güzel hata mesajlarını (ErrorMessage) seçip alıyoruz
                var errors = validationException.Errors.Select(e => e.ErrorMessage);

                // Şık bir JSON objesi oluşturup içine hataları koyuyoruz
                var result = JsonSerializer.Serialize(new { ValidationErrors = errors });

                await context.Response.WriteAsync(result);
                return; // İşlemi burada kesmesi için boş return koyduk
            }

            // Eğer kurallar dışında, sistemsel/kodsal beklenmedik bir hata fırlarsa (Örn: Veritabanı koptu)
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500

            var genericResult = JsonSerializer.Serialize(new { Message = "Sistemde beklenmeyen bir hata oluştu!" });
            await context.Response.WriteAsync(genericResult);
        }
    }
}
