using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsPro.DataLayer;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {

        // Using dependency injection to get an instance of the UnitOfWork class,
        // which provides access to the repositories for the application!
        private IUnitOfWork Data { get; set; }




        // The constructor takes an IUnitOfWork parameter
        // and assigns it to the private Data fieldd.
        public CustomerController(IUnitOfWork data)
        {
            this.Data = data;
        }




        // Uses the UnitOfWork to get a list of countries from the database,
        // ordered by name, and stores it in the ViewBag for use in the views.
        private void LoadCountries()
        {
            ViewBag.Countries = Data.Countries.List(new QueryOptions<Country>
            {
                OrderBy = c => c.Name
            });
        }


        // Uses the UnitOfWork to get a list of customers from the database,
        // including their associated country information, ordered by
        // last name and then first name, and passes it to the view for display.
        [HttpGet]
        [Route("Customers")]
        public IActionResult List()
        {
            var customers = Data.Customers.List(new QueryOptions<Customer>
            {
                Includes = "Country",
                OrderBy = c => c.LastName,
                ThenOrderBy = c => c.FirstName
            });
            return View(customers);
        }


        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            LoadCountries();
            return View("Edit", new Customer());
        }


        // Uses UnitOfWork to prepare the view for editing an existing customer.
        // It retrieves the customer with the specified ID from the database,
        // including its associated country information, and passes it to the
        // view for editing.
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            LoadCountries();
            var customer = Data.Customers.Get(id);
            return View("Edit", customer);
        }



        // Uses UnitOfWork to save changes to a customer.
        // If the model state is valid,
        // it inserts a new customer or updates an existing one,
        // then saves the changes
        // to the database and redirects to the customer list view.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                    Data.Customers.Insert(customer);
                else
                    Data.Customers.Update(customer);

                Data.Save();
                return RedirectToAction("List", "Customer");
            }
            else
            {
                ViewBag.Action = (customer.CustomerID == 0) ? "Add" : "Edit";
                LoadCountries();
                return View("Edit", customer);
            }
        }


        // Uses UnitOfWork to navigate to the delete confirmation view
        // for a customer.
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = Data.Customers.Get(new QueryOptions<Customer>
            {
                Includes = "Country",
                Where = c => c.CustomerID == id
            });
            return View(customer);
        }



        // Uses UnitOfWork to delete a customer.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Customer customer)
        {
            Data.Customers.Delete(customer);
            Data.Save();
            return RedirectToAction("List", "Customer");
        }
    }
}
