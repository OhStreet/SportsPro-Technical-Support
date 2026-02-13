
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System.Linq; // Needed for OrderBy / ThenBy

namespace SportsPro.Controllers
{
    [Route("customers")]
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

        // GET /customers
        [HttpGet("")]
        public IActionResult List()
        {
            var customers = context.Customers
                .Include(c => c.Country)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();

            return View(customers);
        }

        // GET /customers/add
        [HttpGet("add")]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            LoadCountries();
            return View("Edit", new Customer());
        }

        // GET /customers/edit/5
        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            LoadCountries();

            var customer = context.Customers.Find(id);
            if (customer == null) return NotFound();

            return View("Edit", customer);
        }

        // POST /customers/edit
        [HttpPost("edit")]
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
                return RedirectToAction(nameof(List));
            }
            else
            {
                ViewBag.Action = (customer.CustomerID == 0) ? "Add" : "Edit";
                LoadCountries();
                return View("Edit", customer);
            }
        }

        // GET /customers/delete/5
        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var customer = context.Customers
                .Include(c => c.Country)
                .FirstOrDefault(c => c.CustomerID == id);

            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST /customers/delete
        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Customer customer)
        {
            context.Customers.Remove(customer);
            context.SaveChanges();
            return RedirectToAction(nameof(List));
        }
    }
}
