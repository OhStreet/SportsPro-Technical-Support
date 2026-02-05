
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YourApp.Data;
using YourApp.Models;
using YourApp.ViewModels;

namespace YourApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _db;

        public CustomerController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Customer
        public async Task<IActionResult> Index()
        {
            var customers = await _db.Customers
                .Include(c => c.Country)
                .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                .ToListAsync();

            return View(customers);
        }

        // GET: /Customer/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CustomerFormViewModel
            {
                Customer = new Customer(),
                Countries = await GetCountriesSelectListAsync()
            };
            return View("CustomerForm", vm);
        }

        // POST: /Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            // Country must be selected (value 0 means placeholder)
            if (customer.CountryId <= 0)
            {
                ModelState.AddModelError(nameof(Customer.CountryId), "Country is required.");
            }

            if (!ModelState.IsValid)
            {
                var vmInvalid = new CustomerFormViewModel
                {
                    Customer = customer,
                    Countries = await GetCountriesSelectListAsync(customer.CountryId)
                };
                return View("CustomerForm", vmInvalid);
            }

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
            TempData["Message"] = "Customer added successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            var vm = new CustomerFormViewModel
            {
                Customer = customer,
                Countries = await GetCountriesSelectListAsync(customer.CountryId) // pre-select correct country
            };
            return View("CustomerForm", vm);
        }

        // POST: /Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id) return BadRequest();

            if (customer.CountryId <= 0)
            {
                ModelState.AddModelError(nameof(Customer.CountryId), "Country is required.");
            }

            if (!ModelState.IsValid)
            {
                var vmInvalid = new CustomerFormViewModel
                {
                    Customer = customer,
                    Countries = await GetCountriesSelectListAsync(customer.CountryId)
                };
                return View("CustomerForm", vmInvalid);
            }

            try
            {
                _db.Entry(customer).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Customers.AnyAsync(c => c.Id == id))
                    return NotFound();
                throw;
            }

            TempData["Message"] = "Customer updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _db.Customers
                .Include(c => c.Country)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            return View(customer); // confirmation page
        }

        // POST: /Customer/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();

            TempData["Message"] = "Customer deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/Cancel
        [HttpGet]
        public IActionResult Cancel()
        {
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetCountriesSelectListAsync(int? selectedId = null)
        {
            var countries = await _db.Countries
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = selectedId.HasValue && c.Id == selectedId.Value
                })
                .ToListAsync();

            // Insert placeholder at the top
            countries.Insert(0, new SelectListItem { Value = "0", Text = "-- Select country --" });
            return countries;
        }
    }
}
