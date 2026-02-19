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
        private void LoadDropDowns(IncidentFormViewModel model)
        {
            model.Customers = context.Customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();

            model.Products = context.Products
                .OrderBy(p => p.Name)
                .ToList();

            // Same placeholder -1 filter here
            model.Technicians = context.Technicians
                .Where(t => t.TechnicianID > 0)
                .OrderBy(t => t.Name)
                .ToList();
        }

        // Using a ViewModel here to allow for future filtering by status, etc.
        // and cleaner code in the view.
        [HttpGet]
        [Route("Incidents")]
        public ViewResult List(IncidentListViewModel model)
        {
            IQueryable<Incident> query = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .OrderBy(i => i.DateOpened);

            model.Incidents = query.ToList();

            return View(model);
        }

        // Using a view model here for the Add/Edit views to allow for dropdown lists and cleaner code.
        [HttpGet]
        public ViewResult Add()
        {
            var model = new IncidentFormViewModel
            {
                OperationMode = "Add",
                CurrentIncident = new Incident()
            };

            LoadDropDowns(model);

            return View("Edit", model);
        }


        // Using a view model here for the Add/Edit views to allow for dropdown lists and cleaner code.
        [HttpGet]
        public ViewResult Edit(int id)
        {
            var incident = context.Incidents.Find(id);

            var model = new IncidentFormViewModel
            {
                OperationMode = "Edit",
                CurrentIncident = incident
            };

            LoadDropDowns(model);

            return View("Edit", model);
        }

        // Using a view model here for the Edit POST action to allow for dropdown lists and cleaner code.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IncidentFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CurrentIncident.IncidentID == 0)
                {
                    context.Incidents.Add(model.CurrentIncident);
                }
                else
                {
                    context.Incidents.Update(model.CurrentIncident);
                }

                context.SaveChanges();
                return RedirectToAction("List");
            }

            // If validation fails, reload dropdowns
            LoadDropDowns(model);
            return View("Edit", model);
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

            return View("Delete", incident);
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
