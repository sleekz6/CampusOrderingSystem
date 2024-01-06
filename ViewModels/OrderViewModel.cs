using CampusOrdering.Models;

namespace CampusOrdering.ViewModels
{
    public class OrderViewModel
    {

        public Order order { get; set; }
        public IEnumerable<CartItem> items { get; set; }
    }
}
