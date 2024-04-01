using System.ComponentModel.DataAnnotations.Schema;
namespace CampusOrdering.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime PurchaseDateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItem> PurchasedItems { get; set; }
        public AppUser? purchasingUser { get; set; }
        public string? GuestName   { get; set; }
        public Boolean isServed { get; set; }
        public Boolean isRemoved { get; set; }
        public String JSONstring { get; set; }
    }
}
