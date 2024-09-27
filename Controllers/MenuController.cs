using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantMVC.Models;

namespace RestaurantMVC.Controllers
{
	public class MenuController : Controller
	{
		private readonly string baseUrl = "http://localhost:5107/api/Menu"; //Lägg till här
		private readonly HttpClient _client;

        public MenuController()
        {
            _client = new HttpClient();
        }

        public async Task<IActionResult> Index()
		{
            var response = await _client.GetAsync(baseUrl);
            var json = await response.Content.ReadAsStringAsync();
            var menu = JsonConvert.DeserializeObject<List<DishViewModel>>(json);

			var availableMenu = menu.Where(d => d.isAvailable == true).ToList();

			return View(availableMenu);
		}

		public async Task<IActionResult> AdminMenuHandler()
		{
            var response = await _client.GetAsync(baseUrl);
            var json = await response.Content.ReadAsStringAsync();
            var menu = JsonConvert.DeserializeObject<List<DishViewModel>>(json);

			if(!response.IsSuccessStatusCode)
			{
				
			}

            return View(menu);
		}

		public async Task<IActionResult> UpdateDish(int id)
		{

		}
	}
}
