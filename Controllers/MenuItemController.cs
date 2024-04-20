using Microsoft.AspNetCore.Mvc;
using CampusOrdering.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index()
        {
            var menuItems = _context.MenuItems.Include(m => m.Restaurant);
            return View(menuItems.ToList());
        }

        // GET: MenuItem/Create
        public IActionResult Create()
        {
            var restaurants = _context.Restaurants; // Or however you fetch your restaurants
            ViewBag.Restaurants = new SelectList(restaurants, "Id", "Name"); // Adjust "Id" and "Name" as per your Restaurant model properties
            return View();
        }

        // POST: MenuItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ItemName,Price,RestaurantId,ImageUrl,Calories,Size,Category")] MenuItem menuItem)
        {
           // if (ModelState.IsValid)
           // {
                _context.Add(menuItem);
                _context.SaveChanges();
                return RedirectToAction("Menu", new { restID = menuItem.RestaurantId });
           // }
           // return View(menuItem);
        }

        // GET: MenuItem/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = GetItem(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ItemName,Price,RestaurantId,ImageUrl,Calories,Size,Category")] MenuItem menuItem)
        {
            if (id != menuItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuItem);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(menuItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menuItem);
        }

        public IActionResult Delete(int restaurantId)
        {
            var menuItems = _context.MenuItems.Where(item => item.RestaurantId == restaurantId).ToList();

            ViewBag.RestaurantId = restaurantId;

            // Pass the menu items to the view
            return View(menuItems);
        }

        // POST: MenuItem/Delete/5
        [HttpPost]
        public IActionResult DeleteConfirmed(int menuItemId, int restID)
        {
            // Retrieve the menu item from the database
            var menuItem = _context.MenuItems.Find(menuItemId);
            if (menuItem == null)
            {
                return NotFound();
            }

            // Delete the menu item
            _context.MenuItems.Remove(menuItem);
            _context.SaveChanges();

            // Redirect back to the Menu page for the same restaurant
            return RedirectToAction("Menu", new { restID = restID });
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}
