using Microsoft.AspNetCore.Mvc;
namespace CampusOrdering.Controllers
{
    public class MenuItemController : Controller
    {
        private ECartDBEntities eCartDBEntities

        public MenuItemController()
        {

        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
