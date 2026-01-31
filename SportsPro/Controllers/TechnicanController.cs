using Microsoft.AspNetCore.Mvc;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
public IActionResult Add()
{
    ViewBag.Action = "Add";
    ViewBag.Categories = new SelectList(context.Categories.OrderBy(c => c.Name), "CategoryId", "Name");
    return View("Edit", new Contact());
}

[HttpGet]
public IActionResult Edit(int id)
{
    ViewBag.Action = "Edit";
    var contact = context.Contacts.Find(id);
    ViewBag.Categories = new SelectList(context.Categories.OrderBy(c => c.Name), "CategoryId", "Name", contact.CategoryId);
    return View(contact);
}

[HttpPost]
public IActionResult Edit(Contact contact)
{
    if (ModelState.IsValid)
    {
        if (contact.ContactId == 0)
        {
            context.Contacts.Add(contact);
        }
        else
        {
            context.Contacts.Update(contact);
        }

        context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }
    else
    {
        ViewBag.Action = (contact.ContactId == 0) ? "Add" : "Edit";
        ViewBag.Categories = new SelectList(context.Categories.OrderBy(c => c.Name), "CategoryId", "Name", contact.CategoryId);
        return View(contact);
    }
}

[HttpGet]
public IActionResult Delete(int id)
{
    var contact = context.Contacts
        .Include(c => c.Category)
        .FirstOrDefault(c => c.ContactId == id);

    return View(contact);
}

[HttpPost]
public IActionResult Delete(Contact contact)
{
    context.Contacts.Remove(contact);
    context.SaveChanges();
    return RedirectToAction("Index", "Home");
}

    }
}
