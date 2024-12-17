using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantMVC.Models;
using RestaurantMVC.Controllers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace RestaurantMVC.Controllers
{
	public class MenuController : Controller
	{
		private readonly string baseUrl = "http://localhost:5107/api/Menu";
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

		public async Task<IActionResult> AdminDishHandler(string? error)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync(baseUrl);
            var json = await response.Content.ReadAsStringAsync();
            var menu = JsonConvert.DeserializeObject<List<DishViewModel>>(json);

			if(!response.IsSuccessStatusCode)
			{
				return RedirectToAction("Error", "HomeController");
			}

            return View(menu);
		}

		[Authorize]
		public async Task<IActionResult> UpdateDish(int id)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"{baseUrl}/{id}");
			var json = await response.Content.ReadAsStringAsync();
			var dish = JsonConvert.DeserializeObject<DishViewModel>(json);
			return View(dish);
		}


		[Authorize]	
		[HttpPost]
		public async Task<IActionResult> UpdateDish(DishViewModel dish)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (!ModelState.IsValid)
			{
				return View(dish);
			}

			var json = JsonConvert.SerializeObject(dish);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync($"{baseUrl}/update/{dish.id}", content);

			if(!response.IsSuccessStatusCode)
			{
				return View(dish);
			}

			return RedirectToAction("AdminDishHandler");
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> DeleteDish(int id)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.DeleteAsync($"{baseUrl}/delete/{id}");

			if (!response.IsSuccessStatusCode)
			{
				return View();
			}

			return RedirectToAction("AdminDishHandler");
		}


		public IActionResult AddDish()
		{
			return View();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddDish(DishViewModel dish)
		{

			var token = HttpContext.Request.Cookies["JwtToken"];
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			if(!ModelState.IsValid)
			{
				return RedirectToAction("AdminDishHandler"); //Error
			}

			var json = JsonConvert.SerializeObject(dish);
			var content = new StringContent(json, Encoding.UTF8 , "application/json");
			await _client.PostAsync($"{baseUrl}/createdish", content);

			return RedirectToAction("AdminDishHandler"); //Created!
		}
	}
}
