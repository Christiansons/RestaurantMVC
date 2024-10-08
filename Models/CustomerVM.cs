namespace RestaurantMVC.Models
{
	public class CustomerVM
	{
		public int id { get; set; }
		public string name { get; set; }
		public string phoneNr { get; set; }
		public List<reservationVM>? reservationDTOs { get; set; }
	}
}
