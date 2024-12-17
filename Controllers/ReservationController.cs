using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantMVC.Models;
using System.Net.Http.Headers;
using System.Text;

namespace RestaurantMVC.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HttpClient _client;
		private readonly string baseUrl = "http://localhost:5107/api/Reservation";
		public ReservationController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

		[Authorize]
		public async Task<IActionResult> AdminReservationHandler()
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync(baseUrl);
			var json = await response.Content.ReadAsStringAsync();
			var reservations = JsonConvert.DeserializeObject<List<reservationVM>>(json);

			return View(reservations);
		}

		[Authorize]
        public async Task<IActionResult> EditReservation(int reservationId)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"{baseUrl}/{reservationId}");
            var content = await response.Content.ReadAsStringAsync();
            var reservation = JsonConvert.DeserializeObject<reservationVM>(content);
            return View();
        }

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> EditReservation(reservationVM reservationToEdit)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (!ModelState.IsValid)
			{
				return View();
			}

			var json = JsonConvert.SerializeObject(reservationToEdit);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _client.PatchAsync($"{baseUrl}/{reservationToEdit.reservationNumber}", content);

			if (!response.IsSuccessStatusCode)
			{
				return View();
			}

			return RedirectToAction("AdminReservationHandler");
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> DeleteReservation(int reservationNumber)
		{
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.DeleteAsync($"{baseUrl}/delete/{reservationNumber}");

			if (!response.IsSuccessStatusCode)
			{
				return View();
			}

			return RedirectToAction("AdminReservationHandler");
		}

		[Authorize]
		[HttpPost]
        public async Task<IActionResult> DeleteCustomerReservation(int reservationNumber)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.DeleteAsync($"{baseUrl}/delete/{reservationNumber}");
			
			if (!response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index", "home");
			}

			return RedirectToAction("AdminCustomerHandler", "Customer");
		}
    }
}
