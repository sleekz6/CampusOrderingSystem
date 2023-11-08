namespace CampusOrdering.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
        public string ImageUrl { get; set; }
    }
}
