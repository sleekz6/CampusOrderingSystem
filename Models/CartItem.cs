namespace CampusOrdering.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public String MenuItemName { get; set; }
        public string? ImageURL { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
