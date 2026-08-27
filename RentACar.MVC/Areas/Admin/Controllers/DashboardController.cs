using Microsoft.AspNetCore.Mvc;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")] // İşte bu yaka kartı!
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
