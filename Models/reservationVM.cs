namespace RestaurantMVC.Models
{
	public class reservationVM
	{
        public int reservationNumber { get; set; }
		public int partySize { get; set;}
        public int tableNr { get; set; }
        public string customerName { get; set; }
        public string phoneNr { get; set; }
        public DateTime timeFrom { get; set; }
        public DateTime timeTo { get; set; }
    }
}
