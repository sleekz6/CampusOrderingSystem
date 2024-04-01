using System.Web;
namespace CampusOrdering.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
        public string ImageUrl { get; set; }
        public int Calories { get; set; }
        public string Size { get; set; }
        public string Category { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
