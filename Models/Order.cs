using System.ComponentModel.DataAnnotations.Schema;
namespace CampusOrdering.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime PurchaseDateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItem> PurchasedItems { get; set; }
        public Customer purchasingCustomer { get; set; }
        public Boolean isServed { get; set; }
    }
}
