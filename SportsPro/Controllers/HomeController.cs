using Microsoft.AspNetCore.Mvc;

namespace SportsPro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
        
        ViewData["Message"] = "SportsPro helps manage customer incidents and product support workflows.";
            ViewData["Version"] = "v1.0.0";
            return View();
        }

    }

}
