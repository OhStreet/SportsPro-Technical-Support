
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
        public ViewResult List()
        {
            // Get all products ordered by release date
            var products = context.Products.OrderBy(p => p.ReleaseDate).ToList();
            return View(products);
        }

        [HttpGet]
        public ViewResult Add()
        {
            // Tell the shared Edit view we are adding a product
            ViewBag.Action = "Add";
            // Reuse the Edit view with a new empty Product
            return View("Edit", new Product());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            // Tell the shared Edit view we are editing
            ViewBag.Action = "Edit";
            // Find the product by primary key
            var product = context.Products.Find(id);
            return View("Edit", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            // Only save if validation passes
            if (ModelState.IsValid)
            {
                bool isAdd = product.ProductID == 0;

                // New or update
                if (isAdd)
                {
                    context.Products.Add(product);
                }
                else
                {
                    context.Products.Update(product);
                }

                // Commit changes
                context.SaveChanges();

                // ✅ TempData success message (shown after redirect)
                TempData["SuccessMessage"] = isAdd
                    ? "Product added successfully!"
                    : "Product updated successfully!";

                // Go back to the product list
                return RedirectToAction("List", "Product"); // RedirectToActionResult at runtime
            }
            else
            {
                // Reset the action name if validation fails
                ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
                // Redisplay the form with validation errors
                return View("Edit", product); // ViewResult at runtime
            }
        }

        [HttpGet]
        public ViewResult Delete(int id)
        {
            // Grab the product to confirm deletion
            var product = context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult Delete(Product product)
        {
            // Remove the product from the database
            context.Products.Remove(product);
            // Save deletion
            context.SaveChanges();

            // ✅ TempData success message (shown after redirect)
            TempData["SuccessMessage"] = "Product deleted successfully!";

            // Back to the product list
            return RedirectToAction("List", "Product");
        }
    }
}
