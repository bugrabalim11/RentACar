using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RentACar.MVC.Areas.Admin.Controllers
{
    [Area("Admin")] // İşte bu yaka kartı!
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
