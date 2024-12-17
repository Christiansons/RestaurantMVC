using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantMVC.Models;
using System.Net.Http.Headers;
using System.Text;

namespace RestaurantMVC.Controllers
{
    public class CustomerController : Controller
    {
		private readonly string baseUrl = "http://localhost:5107/api/customer";
		private readonly HttpClient _client;
        public CustomerController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

		[Authorize]
		public async Task<IActionResult> AdminCustomerHandler(string? error)
		{
			//Add jwt-token to header
			var token = HttpContext.Request.Cookies["jwtToken"];
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.GetAsync(baseUrl);
			var json = await response.Content.ReadAsStringAsync();
			var customers = JsonConvert.DeserializeObject<List<CustomerVM>>(json);

			if (!response.IsSuccessStatusCode)
			{
				return RedirectToAction("Error", "HomeController");
			}
			
			return View(customers);
		}

		[Authorize]
		public async Task<IActionResult> EditCustomer(int id)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{baseUrl}/{id}");
			var json = await response.Content.ReadAsStringAsync();
			var dish = JsonConvert.DeserializeObject<CustomerVM>(json);
			return View(dish);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> EditCustomer(CustomerVM customer)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!ModelState.IsValid)
			{
				return View(customer);
			}

			var json = JsonConvert.SerializeObject(customer);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync($"{baseUrl}/update/{customer.id}", content);

			if (!response.IsSuccessStatusCode)
			{
				return View(customer);
			}

			return RedirectToAction("AdminCustomerHandler");
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> DeleteCustomer(int id)
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

		[Authorize]
		public IActionResult AddCustomer()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddCustomer(CustomerVM customer)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (!ModelState.IsValid)
			{
				return RedirectToAction("AdminCustomerHandler"); //Error
			}

			var json = JsonConvert.SerializeObject(customer);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			await _client.PostAsync($"{baseUrl}/createCustomer", content);

			return RedirectToAction("AdminCustomerHandler"); //Created!
		}
	}
}
