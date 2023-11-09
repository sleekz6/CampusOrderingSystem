using Microsoft.AspNetCore.Mvc;
using CampusOrdering.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusOrdering.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly OrderingContext _context;

        public MenuItemController(OrderingContext context)
        {
            _context = context;
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
    }
}
