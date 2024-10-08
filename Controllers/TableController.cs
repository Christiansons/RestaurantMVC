using Microsoft.AspNetCore.Mvc;

namespace RestaurantMVC.Controllers
{
	public class TableController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
