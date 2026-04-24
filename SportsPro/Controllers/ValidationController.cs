using Microsoft.AspNetCore.Mvc;
using SportsPro.DataLayer;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ValidationController : Controller
    {

        // Using the Unit of Work pattern for data access
        private IUnitOfWork Data { get; set; }


        // Constructor injection of the Unit of Work
        public ValidationController(IUnitOfWork data)
        {
            this.Data = data;
        }

        // Remote validation checks if the email is already used by
        // another customer.
        // AdditionalFields passes CustomerID so edits don't falsely
        // flag their own email.
        public JsonResult CheckEmail(string email, int customerId)
        {
            var existingCustomer = Data.Customers.Get(new QueryOptions<Customer>
            {
                Where = c => c.Email == email
            });

            if (existingCustomer == null || existingCustomer.CustomerID == customerId)
                return Json(true);
            else
                return Json($"The email '{email}' is already used by another customer.");
        }
    }
}
