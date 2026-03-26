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
        // and cleaner code in the view. Also using subtype ViewResult
        [HttpGet]
        [Route("Incidents")]
        public ViewResult List(IncidentListViewModel model)
        {
            IQueryable<Incident> query = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician);

            if (model.IncidentStatus == "unassigned")
            {
                query = query.Where(i => i.TechnicianID == -1);
            }
            else if (model.IncidentStatus == "open")
            {
                query = query.Where(i => i.DateClosed == null);
            }

            model.Incidents = query.OrderBy(i => i.DateOpened).ToList();

            return View(model);
        }

        // Using a view model here for the Add/Edit views to allow for dropdown lists and cleaner code.
        // Also using subtype ViewResult
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
        // Also using subtype ViewResult
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
        // This may return either ViewResult or RedirectToActionResult so leaving it as 
        // IActionResult
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
        // Using subtype ViewResult
        [HttpGet]
        public ViewResult Delete(int id)
        {
            var incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            return View("Delete", incident);
        }

        // Using subtype RedirectToActionResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("List", "Incident");
        }



        // ****************** TECHNICIAN UPDATE INCIDENT LOGIC ***************** //

        // This section encapsulates the logic for a technician to
        // to view the incidents that are assigned to them. 


        // Remove technician from session and
        // redirect to GetTechnician to select a new one
        public IActionResult SwitchTechnician()
        {
            var spSession = new SportsProSession(HttpContext.Session);
            spSession.RemoveTechnician();

            return RedirectToAction("GetTechnician");
        }


        // GET action to display technician selection form
        [HttpGet("techincident")]
        public IActionResult GetTechnician()
        {
            ViewBag.Technicians = context.Technicians
                .Where(t => t.TechnicianID != -1)
                .OrderBy(t => t.Name)
                .ToList();

            return View();
        }


        // POST action to handle technician selection form submission
        [HttpPost("techincident")]
        public IActionResult GetTechnician(int technicianId)
        {
            if (technicianId == 0)
            {
                ModelState.AddModelError("", "Please select a technician.");

                ViewBag.Technicians = context.Technicians
                    .Where(t => t.TechnicianID != -1)
                    .OrderBy(t => t.Name)
                    .ToList();

                return View();
            }

            var spSession = new SportsProSession(HttpContext.Session);
            spSession.SetTechnicianId(technicianId);

            return RedirectToAction("ListByTech");
        }


        // GET action to display list of incidents
        // assigned to the logged-in technician
        [HttpGet("techincident/list")]
        public IActionResult ListByTech()
        {
            var spSession = new SportsProSession(HttpContext.Session);
            var technicianid = spSession.GetTechnicianId();

            if (technicianid == null)
            {
                return RedirectToAction("GetTechnician");
            }

            var model = new IncidentListViewModel();

            var incidents = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .Where(i => i.TechnicianID == technicianid && i.DateClosed == null)
                .OrderBy(i => i.DateOpened)
                .ToList();

            model.Technician = context.Technicians.Find(technicianid);
            model.Incidents = incidents;

            return View(model);
        }



        // GET action to display edit form for a
        // specific incident assigned to the technician
        [HttpGet("techincident/edit/{id}")]
        public IActionResult EditTech(int id)
        {
            var incident = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefault(i => i.IncidentID == id);

            return View(incident);
        }


        // POST action to handle edit form submission for a
        // specific incident assigned to the technician
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTech(Incident incident)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Update(incident);
                context.SaveChanges();

                return RedirectToAction("ListByTech");
            }

            incident = context.Incidents
            .Include(i => i.Customer)
            .Include(i => i.Product)
            .Include(i => i.Technician)
            .FirstOrDefault(i => i.IncidentID == incident.IncidentID);

            return View(incident);
        }
    }
}
