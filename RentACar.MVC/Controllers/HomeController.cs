using Microsoft.AspNetCore.Mvc;

namespace RentACar.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
