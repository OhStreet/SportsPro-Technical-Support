
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System.Linq; // ensure this is present for OrderBy

namespace SportsPro.Controllers
{
    [Route("products")]
    public class ProductController : Controller
    {
        // Database context for accessing Products table
        private SportsProContext context { get; set; }

        // Constructor injection of the DbContext
        public ProductController(SportsProContext ctx)
        {
            this.context = ctx;
        }

        // GET /products
        [HttpGet("")]
        public IActionResult List()
        {
            // Get all products ordered by release date
            var products = context.Products
                                  .OrderBy(p => p.ReleaseDate)
                                  .ToList();
            return View(products);
        }

        // GET /products/add
        [HttpGet("add")]
        public IActionResult Add()
        {
            // Tell the shared Edit view we are adding a product
            ViewBag.Action = "Add";

            // Reuse the Edit view with a new empty Product
            return View("Edit", new Product());
        }

        // GET /products/edit/5
        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            // Tell the shared Edit view we are editing
            ViewBag.Action = "Edit";

            // Find the product by primary key
            var product = context.Products.Find(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST /products/edit
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
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
                return RedirectToAction(nameof(List));
            }
            else
            {
                // Reset the action name if validation fails
                ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";

                // Redisplay the form with validation errors
                return View(product);
            }
        }

        // GET /products/delete/5
        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            // Grab the product to confirm deletion
            var product = context.Products.Find(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST /products/delete
        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product product)
        {
            // Remove the product from the database
            context.Products.Remove(product);

            // Save deletion
            context.SaveChanges();

            // Back to the product list
            return RedirectToAction(nameof(List));
        }
    }
}
