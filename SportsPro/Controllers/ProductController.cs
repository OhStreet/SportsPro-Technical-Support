using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        // Database context for accessing Products table
        private SportsProContext context { get; set; }

        // Constructor injection of the DbContext
        public ProductController(SportsProContext ctx)
        {
            this.context = ctx;
        }

        [HttpGet]
        [Route("Products")]
        public IActionResult List()
        {
            // Get all products ordered by release date
            var products = context.Products.OrderBy(p => p.ReleaseDate).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            // Tell the shared Edit view we are adding a product
            ViewBag.Action = "Add";

            // Reuse the Edit view with a new empty Product
            return View("Edit", new Product());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Tell the shared Edit view we are editing
            ViewBag.Action = "Edit";

            // Find the product by primary key
            var product = context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            // Only save if validation passes
            if (ModelState.IsValid)
            {
                // New product if ID is 0
                if (product.ProductID == 0)
                {
                    context.Products.Add(product);
                }
                // Existing product gets updated
                else
                {
                    context.Products.Update(product);
                }

                // Commit changes to the database
                context.SaveChanges();

                // Go back to the product list
                return RedirectToAction("List", "Product");
            }
            else
            {
                // Reset the action name if validation fails
                ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";

                // Redisplay the form with validation errors
                return View(product);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Grab the product to confirm deletion
            var product = context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            // Remove the product from the database
            context.Products.Remove(product);

            // Save deletion
            context.SaveChanges();

            // Back to the product list
            return RedirectToAction("List", "Product");
        }
    }
}
