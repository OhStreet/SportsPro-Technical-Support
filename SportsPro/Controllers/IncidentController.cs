using Microsoft.AspNetCore.Mvc;
using SportsPro.DataLayer;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        // Here, we are using the Unit of Work pattern,
        // so we have a single property for data access instead of
        // multiple DbSet properties.
        private IUnitOfWork Data { get; set; }

        // Constructor injection of the Unit of Work
        public IncidentController(IUnitOfWork data)
        {
            this.Data = data;
        }

        // Load dropdown lists for customers, products, and technicians
        // using the QueryOptions class to specify sorting and
        // filtering criteria.
        private void LoadDropDowns(IncidentFormViewModel model)
        {
            model.Customers = Data.Customers.List(new QueryOptions<Customer>
            {
                OrderBy = c => c.LastName,
                ThenOrderBy = c => c.FirstName
            }).ToList();

            model.Products = Data.Products.List(new QueryOptions<Product>
            {
                OrderBy = p => p.Name
            }).ToList();

            model.Technicians = Data.Technicians.List(new QueryOptions<Technician>
            {
                Where = t => t.TechnicianID > 0,
                OrderBy = t => t.Name
            }).ToList();
        }


        // using the QueryOptions class to specify
        // sorting and filtering criteria for incidents.
        [HttpGet]
        [Route("Incidents")]
        public ViewResult List(IncidentListViewModel model)
        {
            var options = new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                OrderBy = i => i.DateOpened
            };

            if (model.IncidentStatus == "unassigned")
                options.Where = i => i.TechnicianID == -1;
            else if (model.IncidentStatus == "open")
                options.Where = i => i.DateClosed == null;

            model.Incidents = Data.Incidents.List(options).ToList();

            return View(model);
        }

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


        // Using Unit of Work pattern,
        // we can use the same Edit view for both adding and editing incidents.
        [HttpGet]
        public ViewResult Edit(int id)
        {
            var incident = Data.Incidents.Get(id);
            var model = new IncidentFormViewModel
            {
                OperationMode = "Edit",
                CurrentIncident = incident
            };
            LoadDropDowns(model);
            return View("Edit", model);
        }


        // Using Unit of Work pattern for edit post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IncidentFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CurrentIncident.IncidentID == 0)
                    Data.Incidents.Insert(model.CurrentIncident);
                else
                    Data.Incidents.Update(model.CurrentIncident);

                Data.Save();
                return RedirectToAction("List");
            }

            LoadDropDowns(model);
            return View("Edit", model);
        }

        // Using Unit of Work pattern for delete confirmation view
        [HttpGet]
        public ViewResult Delete(int id)
        {
            var incident = Data.Incidents.Get(new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                Where = i => i.IncidentID == id
            });
            return View("Delete", incident);
        }

        // Using Unit of Work pattern for delete post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult Delete(Incident incident)
        {
            Data.Incidents.Delete(incident);
            Data.Save();
            return RedirectToAction("List", "Incident");
        }

        // ****************** TECHNICIAN UPDATE INCIDENT LOGIC ***************** //

        public IActionResult SwitchTechnician()
        {
            var spSession = new SportsProSession(HttpContext.Session);
            spSession.RemoveTechnician();
            return RedirectToAction("GetTechnician");
        }

        // Using Unit of Work pattern to get a list of
        // technicians for the dropdown
        [HttpGet]
        public IActionResult GetTechnician()
        {
            ViewBag.Technicians = Data.Technicians.List(new QueryOptions<Technician>
            {
                Where = t => t.TechnicianID != -1,
                OrderBy = t => t.Name
            });
            return View();
        }

        // Using Unit of Work pattern to set the selected technician in session
        [HttpPost]
        public IActionResult GetTechnician(int technicianId)
        {
            if (technicianId == 0)
            {
                ModelState.AddModelError("", "Please select a technician.");

                ViewBag.Technicians = Data.Technicians.List(new QueryOptions<Technician>
                {
                    Where = t => t.TechnicianID != -1,
                    OrderBy = t => t.Name
                });

                return View();
            }

            var spSession = new SportsProSession(HttpContext.Session);
            spSession.SetTechnicianId(technicianId);
            return RedirectToAction("ListByTech");
        }

        // Using Unit of Work pattern to get a list of incidents
        // for the selected technician
        [HttpGet]
        public IActionResult ListByTech()
        {
            var spSession = new SportsProSession(HttpContext.Session);
            var technicianId = spSession.GetTechnicianId();

            if (technicianId == null)
                return RedirectToAction("GetTechnician");

            // Two WHERE clauses: filter by technician AND open incidents (chapter 13 pattern)
            var options = new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                OrderBy = i => i.DateOpened
            };
            options.Where = i => i.TechnicianID == technicianId;
            options.Where = i => i.DateClosed == null;

            var model = new IncidentListViewModel();
            model.Technician = Data.Technicians.Get((int)technicianId);
            model.Incidents = Data.Incidents.List(options).ToList();

            return View(model);
        }


        // Using Unit of Work pattern to get the incident for
        // editing and then updating it
        [HttpGet]
        public IActionResult EditTech(int id)
        {
            var incident = Data.Incidents.Get(new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                Where = i => i.IncidentID == id
            });
            return View(incident);
        }


        // Using Unit of Work pattern to update the incident after editing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTech(Incident incident)
        {
            if (ModelState.IsValid)
            {
                Data.Incidents.Update(incident);
                Data.Save();
                return RedirectToAction("ListByTech");
            }

            incident = Data.Incidents.Get(new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                Where = i => i.IncidentID == incident.IncidentID
            }) ?? incident;

            return View(incident);
        }
    }
}
