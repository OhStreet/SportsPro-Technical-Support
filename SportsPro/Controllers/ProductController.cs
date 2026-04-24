using Microsoft.AspNetCore.Mvc;
using SportsPro.DataLayer;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        // Using the Repository pattern,
        // we have a single property for data access instead of
        // multiple DbSet properties.
        private Repository<Product> products { get; set; }


        // Constructor injection of the Repository
        public ProductController(IUnitOfWork data)
        {
            products = data.Products;
        }


        // using the QueryOptions class to specify ordering by ReleaseDate
        [HttpGet]
        [Route("Products")]
        public ViewResult List()
        {
            var productList = products.List(new QueryOptions<Product>
            {
                OrderBy = p => p.ReleaseDate
            });
            return View(productList);
        }

        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Product());
        }



        // Using the repository for edit view
        [HttpGet]
        public ViewResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var product = products.Get(id);
            return View("Edit", product);
        }



        // Using the repository to add or update a product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                bool isAdd = product.ProductID == 0;

                if (isAdd)
                    products.Insert(product);
                else
                    products.Update(product);

                products.Save();

                TempData["SuccessMessage"] = isAdd
                    ? "Product added successfully!"
                    : "Product updated successfully!";

                return RedirectToAction("List", "Product");
            }
            else
            {
                ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
                return View("Edit", product);
            }
        }




        // Using the repository to return a product for confirmation of deletion
        [HttpGet]
        public ViewResult Delete(int id)
        {
            var product = products.Get(id);
            return View(product);
        }



        // Using the repository to delete a product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult Delete(Product product)
        {
            products.Delete(product);
            products.Save();

            TempData["SuccessMessage"] = "Product deleted successfully!";
            return RedirectToAction("List", "Product");
        }
    }
}
