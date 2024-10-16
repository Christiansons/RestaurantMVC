using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantMVC.Models;

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

		public async Task<IActionResult> AdminReservationHandler()
		{
			var response = await _client.GetAsync(baseUrl);
			var json = await response.Content.ReadAsStringAsync();
			var reservations = JsonConvert.DeserializeObject<List<reservationVM>>(json);

			return View(reservations);
		}

        public async Task<IActionResult> EditReservation(int reservationId)
        {
            var response = await _client.GetAsync($"{baseUrl}/{reservationId}");
            var content = await response.Content.ReadAsStringAsync();
            var reservation = JsonConvert.DeserializeObject<reservationVM>(content);
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> EditReservation()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> DeleteReservation(int reservationNumber)
		{
			var response = await _client.DeleteAsync($"{baseUrl}/delete/{reservationNumber}");

			if (!response.IsSuccessStatusCode)
			{
				return View();
			}

			return RedirectToAction("AdminReservationHandler");
		}

		[HttpPost]
        public async Task<IActionResult> DeleteCustomerReservation(int reservationNumber)
        {
			var response = await _client.DeleteAsync($"{baseUrl}/delete/{reservationNumber}");
			
			if (!response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index", "home");
			}

			return RedirectToAction("AdminCustomerHandler", "Customer");
		}
    }
}
