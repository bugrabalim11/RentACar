using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentACar.Core.Exceptions;

namespace RentACar.API.Filters
{
    /// <summary>
    /// API'ye gelen tüm istekleri (Request) Controller'a ulaşmadan önce araya girerek denetler.
    /// FluentValidation veya DataAnnotations kurallarına uymayan bir model tespit edildiğinde,
    /// standartlaştırılmış ErrorDetails formatında 400 Bad Request döner.
    /// </summary>
    public class ValidationFilters : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var erroDetails = new ErrorDetails
                {
                    StatusCode = 400,
                    Message = "Doğrulama kuralı ihlali! Lütfen girdiğiniz bilgileri kontrol edin.",
                    ValidationErrors = errors 
                };
                context.Result = new BadRequestObjectResult(erroDetails);
                return;
            }
            await next();
        }
    }
}
