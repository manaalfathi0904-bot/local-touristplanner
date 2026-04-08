using Microsoft.AspNetCore.Mvc;

namespace TouristPlanner.Controllers
{
    public class PageController : Controller
    {
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Plan()
        {
            return View();
        }
    }
}