using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CampusOrdering.Models;
namespace CampusOrdering.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly OrderingContext _context;

        public RestaurantController(OrderingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Restaurant> restaurants = _context.Restaurants.ToList();
            return View(restaurants);
        }

        public IActionResult Display(string LogoUrl)
        {
            Restaurant restaurant = _context.Restaurants.Find(LogoUrl);
            if (restaurant == null)
                return NotFound();
            return View(restaurant);
        }



    }
}
