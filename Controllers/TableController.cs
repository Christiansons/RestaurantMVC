using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantMVC.Models;
using System.Net.Http.Headers;
using System.Text;

namespace RestaurantMVC.Controllers
{
	public class TableController : Controller
	{
		private readonly string baseUrl = "http://localhost:5107/api/Table";
		private readonly HttpClient _client;
        public TableController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> AdminTableHandler(string? error)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync(baseUrl);
			var json = await response.Content.ReadAsStringAsync();
			var tables = JsonConvert.DeserializeObject<List<TableVM>>(json);

			if (!response.IsSuccessStatusCode)
			{
				return RedirectToAction("Error", "HomeController");
			}

			return View(tables);
		}

		[Authorize]
		public async Task<IActionResult> EditTable(int tableNr)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"{baseUrl}/{tableNr}");
			var json = await response.Content.ReadAsStringAsync();
			var table = JsonConvert.DeserializeObject<TableVM>(json);
			return View(table);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> EditTable(TableVM table)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (!ModelState.IsValid)
			{
				return View(table);
			}

			var json = JsonConvert.SerializeObject(table);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync($"{baseUrl}/update/{table.tableNr}", content);

			if (!response.IsSuccessStatusCode)
			{
				return View(table);
			}

			return RedirectToAction("AdminDishHandler");
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> DeleteTable(int tableNr)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.DeleteAsync($"{baseUrl}/delete/{tableNr}");

			if (!response.IsSuccessStatusCode)
			{
				return View();
			}

			return RedirectToAction("AdminDishHandler");
		}


		[Authorize]
		public IActionResult AddTable()
		{

			return View();
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddTable(TableVM table)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (!ModelState.IsValid)
			{
				return RedirectToAction("AdminDishHandler"); //Error
			}

			var json = JsonConvert.SerializeObject(table);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			await _client.PostAsync($"{baseUrl}/createtable", content);

			return RedirectToAction("AdminDishHandler"); //Created!
		}
	}
}
