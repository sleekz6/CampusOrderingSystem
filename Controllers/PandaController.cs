namespace CampusOrdering.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    public class PandaController : Controller
    {
        public IActionResult PandaView()
        {
            return View();
        }
    }
}