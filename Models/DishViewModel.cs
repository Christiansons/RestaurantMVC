namespace RestaurantMVC.Models
{
    public class DishViewModel
    {
        public int id { get; set; }
        public string dishName { get; set; }
        public int priceInSek { get; set; }
        public bool isAvailable { get; set; }
    }
}
