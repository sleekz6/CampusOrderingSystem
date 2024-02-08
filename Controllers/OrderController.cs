using CampusOrdering.Data;
using CampusOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusOrdering.Controllers
{
    public class OrderController : Controller
    {

        private readonly AuthDbContext _context;


        public OrderController(AuthDbContext context)
        {
            _context = context;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.purchasingCustomer)
                .ToList();
            return View(orders);
        }

        public IActionResult ServeOrder(int orderId) { 
        
          var order =   _context.Orders.FirstOrDefault( m => m.OrderId == orderId);

            order.isServed = true;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
