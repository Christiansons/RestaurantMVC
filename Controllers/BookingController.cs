using Microsoft.AspNetCore.Mvc;

namespace RestaurantMVC.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
