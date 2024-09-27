using Microsoft.AspNetCore.Mvc;

namespace RestaurantMVC.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
