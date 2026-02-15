using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        // Database context for accessing Technicians table
        private SportsProContext context { get; set; }

        // Constructor injection of the DbContext
        public TechnicianController(SportsProContext ctx)
        {
            this.context = ctx;
        }

        [HttpGet]
        [Route("Technicians")]
        public IActionResult List()
        {
            // Get all technicians except the placeholder (-1), ordered by name
            var technicians = context.Technicians
                                     .Where(t => t.TechnicianID != -1)
                                     .OrderBy(t => t.Name)
                                     .ToList();

            return View(technicians);
        }


        [HttpGet]
        public IActionResult Add()
        {
            // Tell the shared Edit view we are adding a technician
            ViewBag.Action = "Add";

            // Reuse the Edit view with a new empty Technician
            return View("Edit", new Technician());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Tell the shared Edit view we are editing
            ViewBag.Action = "Edit";

            // Find the technician by primary key
            var technician = context.Technicians.Find(id);
            return View(technician);
        }

        [HttpPost]
        public IActionResult Edit(Technician technician)
        {
            // Only save if validation passes
            if (ModelState.IsValid)
            {
                // New technician if ID is 0
                if (technician.TechnicianID == 0)
                {
                    context.Technicians.Add(technician);
                }
                // Existing technician gets updated
                else
                {
                    context.Technicians.Update(technician);
                }

                // Commit changes
                context.SaveChanges();

                // Back to technician list
                return RedirectToAction("List", "Technician");
            }
            else
            {
                // Reset action name if validation fails
                ViewBag.Action = (technician.TechnicianID == 0) ? "Add" : "Edit";
                return View(technician);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Grab technician to confirm deletion
            var technician = context.Technicians.Find(id);
            return View(technician);
        }

        [HttpPost]
        public IActionResult Delete(Technician technician)
        {
            // Remove technician
            context.Technicians.Remove(technician);
            context.SaveChanges();

            return RedirectToAction("List", "Technician");
        }
    }
}
