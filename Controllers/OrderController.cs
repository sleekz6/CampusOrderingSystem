using CampusOrdering.Interfaces;
using CampusOrdering.Migrations;
using CampusOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CampusOrdering.Controllers
{
    public class OrderController : Controller
    {

        private readonly AuthDbContext _context;
        private readonly IUserRepository _userRepository;


        public OrderController(AuthDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

    
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserById(currentUserId);
            
            if (user == null)
            {
                return View("~/Views/Shared/_SignInRequired.cshtml");
            }

            if (user.RestaurantID == null)
            {
                // Redirect to an error page indicating that the user has no genre ID assigned
                return View("~/Views/Shared/_SignInRequired.cshtml");
            }
            var orders = _context.Orders
                 .Include(o => o.purchasingUser) // Include purchasingCustomer navigation property
                 .Where(o => o.PurchasedItems
                 .Any(ci => _context.MenuItems
                         .Any(mi => mi.RestaurantId == user.RestaurantID && mi.ItemName == ci.MenuItemName)))
                 .ToList();

            return View(orders);
        }



        public async Task<IActionResult> Served()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserById(currentUserId);

            if (user == null)
            {
                return View("~/Views/Shared/_SignInRequired.cshtml");
            }

            if (user.RestaurantID == null)
            {
                // Redirect to an error page indicating that the user has no genre ID assigned
                return View("~/Views/Shared/_SignInRequired.cshtml");
            }
            var orders = _context.Orders
                 .Include(o => o.purchasingUser) // Include purchasingCustomer navigation property
                 .Where(o => o.PurchasedItems
                 .Any(ci => _context.MenuItems
                         .Any(mi => mi.RestaurantId == user.RestaurantID && mi.ItemName == ci.MenuItemName)))
                 .ToList();

            return View(orders);
        }


        public IActionResult ServeOrder(int orderId) { 
        
          var order =   _context.Orders.FirstOrDefault( m => m.OrderId == orderId);

            order.isServed = true;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {

            var order = _context.Orders.FirstOrDefault(m => m.OrderId == id);

            order.isRemoved = true;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SearchOrders(string searchTerm)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserById(currentUserId);

           

            var orders = _context.Orders
                 .Include(o => o.purchasingUser) // Include purchasingUser navigation property
                 .Where(o => o.GuestName.Contains(searchTerm) && o.PurchasedItems
                     .Any(ci => _context.MenuItems
                         .Any(mi => mi.RestaurantId == user.RestaurantID && mi.ItemName == ci.MenuItemName)))
                 .ToList();

            return View("Index", orders);
        }



        [HttpPost]
        public async Task<IActionResult> SearchUnservedOrders(string searchTerm)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserById(currentUserId);



            var orders = _context.Orders
                 .Include(o => o.purchasingUser) // Include purchasingUser navigation property
                 .Where(o => o.GuestName.Contains(searchTerm) && o.PurchasedItems
                     .Any(ci => _context.MenuItems
                         .Any(mi => mi.RestaurantId == user.RestaurantID && mi.ItemName == ci.MenuItemName)))
                 .ToList();

            return View("Served", orders);
        }
    }
}
