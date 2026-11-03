using FluentValidation;
using RentACar.Core.Exceptions;
using System.Text.Json;

namespace RentACar.API.Middlewares
{
    /// <summary>
    /// Uygulama genelinde fırlatılan tüm hataları (Exception) yakalayan global hata yakalama (Global Exception Handling) ara katmanıdır.
    /// Hata türüne (Validation, Business, System) göre standartlaştırılmış ErrorDetails modeliyle JSON formatında yanıt döner.
    /// </summary>
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
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            ErrorDetails errorDetails = new ErrorDetails(); 

            if (exception is ValidationException validationException)
            {
                errorDetails.StatusCode = 400;
                errorDetails.Message = "Doğrulama kuralı ihlali!";
                errorDetails.ValidationErrors = validationException.Errors.Select(e => e.ErrorMessage);
            }
            else if (exception is BusinessException businessException)
            {
                errorDetails.StatusCode = 400;
                errorDetails.Message = businessException.Message;
            }
            else
            {
                errorDetails.StatusCode = 500;
                errorDetails.Message = "Sistemde beklemeyen bir hata oluştu!";
            }

            context.Response.StatusCode = errorDetails.StatusCode;
            var result = JsonSerializer.Serialize(errorDetails);
            await context.Response.WriteAsync(result);
        }
    }
}
