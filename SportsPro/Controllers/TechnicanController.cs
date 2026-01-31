using Microsoft.AspNetCore.Mvc;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
public IActionResult Add()
{

}

[HttpGet]
public IActionResult Edit(int id)
{
   
}

[HttpPost]
public IActionResult Edit()
{
   
}

[HttpGet]
public IActionResult Delete(int id)
{
    
}

[HttpPost]
public IActionResult Delete()
{
   
}

    }
}
