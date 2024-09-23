using Microsoft.AspNetCore.Mvc;

namespace RestaurantMVC.Controllers
{
	public class MenuController : Controller
	{
		private readonly string baseUrl = "localhost"; //Lägg till här
		private readonly HttpClient _client;

        public MenuController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
		{
			

			return View();
		}
	}
}
