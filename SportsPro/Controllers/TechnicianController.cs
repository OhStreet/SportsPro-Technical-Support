using Microsoft.AspNetCore.Mvc;
using SportsPro.DataLayer;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {

        // Using the Repository pattern for technicians
        private Repository<Technician> technicians { get; set; }


        // Constructor injection of the Repository for technicians
        public TechnicianController(IUnitOfWork data)
        {
            technicians = data.Technicians;
        }


        // using the QueryOptions class to specify ordering by Name and
        // excluding the default technician with TechnicianID of -1
        [HttpGet]
        [Route("Technicians")]
        public IActionResult List()
        {
            var techList = technicians.List(new QueryOptions<Technician>
            {
                Where = t => t.TechnicianID != -1,
                OrderBy = t => t.Name
            });
            return View(techList);
        }


        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Technician());
        }



        // Using repository for Edit view
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var technician = technicians.Get(id);
            return View(technician);
        }



        // Using repository for Add or edit a technician
        [HttpPost]
        public IActionResult Edit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                if (technician.TechnicianID == 0)
                    technicians.Insert(technician);
                else
                    technicians.Update(technician);

                technicians.Save();
                return RedirectToAction("List", "Technician");
            }
            else
            {
                ViewBag.Action = (technician.TechnicianID == 0) ? "Add" : "Edit";
                return View(technician);
            }
        }


        // Using repository for delete confirmation view of a technician
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var technician = technicians.Get(id);
            return View(technician);
        }


        // Using repository for deletion of a technician
        [HttpPost]
        public IActionResult Delete(Technician technician)
        {
            technicians.Delete(technician);
            technicians.Save();
            return RedirectToAction("List", "Technician");
        }
    }
}
