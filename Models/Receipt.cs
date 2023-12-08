using System.ComponentModel.DataAnnotations.Schema;
namespace CampusOrdering.Models
{
    public class Receipt
    {
        public int ReceiptId { get; set; }
        public DateTime PurchaseDateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItem> PurchasedItems { get; set; }
    }
}
