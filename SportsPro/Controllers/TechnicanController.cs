
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private readonly SportsProContext _context;

        public TechnicianController(SportsProContext context)
        {
            _context = context;
        }

        // GET: /Technician
        public async Task<IActionResult> Index()
        {
            var techs = await _context.Technicians
                .OrderBy(t => t.Name)
                .ToListAsync();

            return View(techs); // Views/Technician/Index.cshtml
        }

        // GET: /Technician/About
        public IActionResult About()
        {
            return View(); // Views/Technician/About.cshtml
        }

        // GET: /Technician/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View(new Technician()); // Views/Technician/Add.cshtml
        }

        // POST: /Technician/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Technician model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Technicians.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Technician/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var tech = await _context.Technicians.FindAsync(id.Value);
            if (tech is null) return NotFound();

            return View(tech); // Views/Technician/Edit.cshtml
        }

        // POST: /Technician/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Technician model)
        {
            if (id != model.TechnicianId) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.Technicians.AnyAsync(t => t.TechnicianId == id);
                if (!exists) return NotFound();
                throw; // rethrow if it was a real concurrency conflict
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Technician/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var tech = await _context.Technicians
                .FirstOrDefaultAsync(t => t.TechnicianId == id.Value);

            if (tech is null) return NotFound();

            return View(tech); // Views/Technician/Delete.cshtml (confirmation)
        }

        // POST: /Technician/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tech = await _context.Technicians.FindAsync(id);
            if (tech is null) return NotFound();

            _context.Technicians.Remove(tech);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
