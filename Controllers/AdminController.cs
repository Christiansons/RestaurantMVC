using Microsoft.AspNetCore.Mvc;

namespace RestaurantMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly string baseUrl = "http://localhost:5107/api/";
        private readonly HttpClient _client;

        public AdminController()
        {
            _client = new HttpClient();
        }

        public IActionResult AdminLogIn()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
