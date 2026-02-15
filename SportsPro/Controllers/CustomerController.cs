using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private SportsProContext context;

        public CustomerController(SportsProContext context)
        {
            this.context = context;
        }

        // Load countries for dropdown
        private void LoadCountries()
        {
            ViewBag.Countries = context.Countries
                .OrderBy(c => c.Name)
                .ToList();
        }

        [HttpGet]
        [Route("Customers")]
        public IActionResult List()
        {
            var customers = context.Customers
                .Include(c => c.Country)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();

            return View(customers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            LoadCountries();
            return View("Edit", new Customer());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            LoadCountries();

            var customer = context.Customers.Find(id);
            return View("Edit", customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            System.Diagnostics.Debug.WriteLine($"CountryID POSTED = '{customer.CountryID}'");
            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                {
                    context.Customers.Add(customer);
                }
                else
                {
                    context.Customers.Update(customer);
                }

                context.SaveChanges();
                return RedirectToAction("List", "Customer");
            }
            else
            {
                ViewBag.Action = (customer.CustomerID == 0) ? "Add" : "Edit";
                LoadCountries();
                return View("Edit", customer);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = context.Customers
                .Include(c => c.Country)
                .FirstOrDefault(c => c.CustomerID == id);

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Customer customer)
        {
            context.Customers.Remove(customer);
            context.SaveChanges();
            return RedirectToAction("List", "Customer");
        }
    }
}
