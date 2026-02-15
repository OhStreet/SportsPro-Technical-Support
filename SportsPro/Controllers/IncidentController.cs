using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

// CRUD logic remains the same from Products/Technicians.
// Only commenting on what is specific to Incidents

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        // DB context
        private SportsProContext context;

        public IncidentController(SportsProContext context)
        {
            this.context = context;
        }

        // Helper function to load dropdowns on Add/Edit
        private void LoadDropDowns()
        {
            ViewBag.Customers = context.Customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();

            ViewBag.Products = context.Products
                .OrderBy(p => p.Name)
                .ToList();

            // Same placeholder -1 filter here
            ViewBag.Technicians = context.Technicians
                .Where(t => t.TechnicianID > 0)
                .OrderBy(t => t.Name)
                .ToList();
        }

        [HttpGet]
        [Route("Incidents")]
        public IActionResult List()
        {
            var incidents = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .OrderBy(i => i.DateOpened)
                .ToList();

            return View(incidents);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            LoadDropDowns();
            return View("Edit", new Incident());
        }

        // Edit GET/POST
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            LoadDropDowns();

            var incident = context.Incidents.Find(id);
            return View("Edit", incident);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Incident incident)
        {

            // DEBUG: what did we actually receive?
            System.Diagnostics.Debug.WriteLine(
                $"POST IncidentID={incident.IncidentID}, CustomerID={incident.CustomerID}, ProductID={incident.ProductID}, TechnicianID={incident.TechnicianID}");

            foreach (var kvp in ModelState)
            {
                foreach (var err in kvp.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"ModelState[{kvp.Key}] => {err.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                if (incident.IncidentID == 0)
                {
                    context.Incidents.Add(incident);
                }
                else
                {
                    context.Incidents.Update(incident);
                }

                context.SaveChanges();

                return RedirectToAction("List", "Incident");
            }
            else
            {
                ViewBag.Action = (incident.IncidentID == 0) ? "Add" : "Edit";
                LoadDropDowns();
                return View("Edit", incident);
            }
        }

        // Delete GET/POST
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            return View(incident);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("List", "Incident");
        }
    }
}
