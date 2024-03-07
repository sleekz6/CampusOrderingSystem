using Microsoft.AspNetCore.Mvc;
using CampusOrdering.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CampusOrdering.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly AuthDbContext _context;

        public MenuItemController(AuthDbContext context)
        {
            _context = context;
        }

        public MenuItem GetItem(int itemID)
        {
            return _context.MenuItems
                .Include(m => m.Restaurant)
                .FirstOrDefault(m => m.Id == itemID);
        }

        public IActionResult Menu(int restID)
        {
            var restaurant = _context.Restaurants
                .Include(r => r.Menu)
                .FirstOrDefault(r => r.Id == restID);
            if (restaurant == null)
                return NotFound();
            return View(restaurant);
        }

        public IActionResult GetItemDetails(int itemId)
        {
            // Retrieve item details from the database or any other data source
            var item = GetItem(itemId);

            // Return a partial view with item details
            return PartialView("_ItemDetailsPartial", item);
        }
    }
}
