using Microsoft.AspNetCore.Mvc;
using SportsPro.DataLayer;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class RegistrationController : Controller
    {

        // Using the Unit of Work pattern,
        private IUnitOfWork data { get; set; }

        public RegistrationController(IUnitOfWork data)
        {
            this.data = data;
        }

        // Redirect /Registration to GetCustomer
        public IActionResult Index()
        {
            return RedirectToAction("GetCustomer");
        }

        // Show the customer selection form
        [HttpGet]
        public IActionResult GetCustomer()
        {
            ViewBag.Customers = data.Customers.List(new QueryOptions<Customer>
            {
                OrderBy = c => c.LastName,
                ThenOrderBy = c => c.FirstName
            });

            return View();
        }

        // Store selected customer in session, redirect to List
        [HttpPost]
        public IActionResult GetCustomer(int customerId)
        {
            if (customerId == 0)
            {
                ModelState.AddModelError("", "Please select a customer.");

                ViewBag.Customers = data.Customers.List(new QueryOptions<Customer>
                {
                    OrderBy = c => c.LastName,
                    ThenOrderBy = c => c.FirstName
                });

                return View();
            }

            var spSession = new SportsProSession(HttpContext.Session);
            spSession.SetCustomerId(customerId);

            return RedirectToAction("List");
        }




        // Show the registrations for the current customer
        [HttpGet]
        public IActionResult List()
        {
            var spSession = new SportsProSession(HttpContext.Session);
            int? customerId = spSession.GetCustomerId();

            if (customerId == null)
            {
                return RedirectToAction("GetCustomer");
            }

            // Load the customer
            var customer = data.Customers.Get((int)customerId);

            // Load products already registered for this customer
            var registrations = data.Registrations.List(new QueryOptions<Registration>
            {
                Where = r => r.CustomerID == customerId,
                Includes = "Product"
            });

            // Load all products for the dropdown
            ViewBag.AllProducts = data.Products.List(new QueryOptions<Product>
            {
                OrderBy = p => p.Name
            });

            ViewBag.Customer = customer;
            ViewBag.Registrations = registrations;

            return View();
        }




        // Register a product for the current customer
        [HttpPost]
        public IActionResult Register(int productId)
        {
            var spSession = new SportsProSession(HttpContext.Session);
            int? customerId = spSession.GetCustomerId();

            if (customerId == null)
            {
                return RedirectToAction("GetCustomer");
            }

            // Check if already registered
            var existing = data.Registrations.Get(new QueryOptions<Registration>
            {
                Where = r => r.CustomerID == customerId && r.ProductID == productId
            });

            if (existing == null)
            {
                data.Registrations.Insert(new Registration
                {
                    CustomerID = (int)customerId,
                    ProductID = productId
                });
                data.Save();
            }

            return RedirectToAction("List");
        }





        // Allow switching to a different customer
        public IActionResult SwitchCustomer()
        {
            var spSession = new SportsProSession(HttpContext.Session);
            spSession.RemoveCustomer();

            return RedirectToAction("GetCustomer");
        }
    }
}
