using Microsoft.AspNetCore.Mvc;

namespace RestaurantMVC.Controllers
{
	public class MenuController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
