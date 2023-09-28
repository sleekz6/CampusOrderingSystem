namespace CampusOrdering.Controllers { 
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

    public class CFAController : Controller 
    {
        public IActionResult CFAView()
        {
            return View();
        }
    }
}
