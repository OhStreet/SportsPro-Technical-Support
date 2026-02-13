
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    // Base route for all actions in this controller.
    // This makes URLs start with /incidents
    [Route("incidents")]
    public class IncidentController : Controller
    {
        // DB context
        private readonly SportsProContext context;

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

        // GET /incidents  (default)
        // GET /incidents/list  (optional, for backwards-compat)
        [HttpGet("")]
        [HttpGet("list")]
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

        // GET /incidents/add
        [HttpGet("add")]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            LoadDropDowns();
            return View("Edit", new Incident());
        }

        // GET /incidents/edit/5
        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            LoadDropDowns();

            var incident = context.Incidents.Find(id);
            return View("Edit", incident);
        }

        // POST /incidents/edit
        [HttpPost("edit")]
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
                    context.Incidents.Add(incident);
                else
                    context.Incidents.Update(incident);

                context.SaveChanges();

                // Works with attribute routing too; uses action discovery.
                return RedirectToAction(nameof(List));
            }
            else
            {
                ViewBag.Action = (incident.IncidentID == 0) ? "Add" : "Edit";
                LoadDropDowns();
                return View("Edit", incident);
            }
        }

        // GET /incidents/delete/5
        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            return View(incident);
        }

        // POST /incidents/delete
        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction(nameof(List));
        }
    }
}
