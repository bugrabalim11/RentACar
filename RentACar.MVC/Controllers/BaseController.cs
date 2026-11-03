using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentACar.MVC.Models.Responses;

namespace RentACar.MVC.Controllers
{
    public class BaseController : Controller
    {
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

        protected async Task<string> GetApiErrorMessageAsync(HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return "Bu işlem için yetkiniz yok. Lütfen giriş yapın!";
            }

            var errorJsonData = await responseMessage.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorDetailsDto>(errorJsonData);
            if (errorData != null)
            {
                if(errorData.ValidationErrors!=null && errorData.ValidationErrors.Any())
                {
                    return string.Join(" ", errorData.ValidationErrors);
                }

                return errorData.Message;
            }

            return "Bilinmeyen bir hata oluştu!";
        }
    }
}