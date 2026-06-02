using Microsoft.AspNetCore.Mvc;
using WeeklyMansion.Data;
using WeeklyMansion.Models;

namespace WeeklyMansion.Controllers
{

    public class PropertyController : Controller
    {
        private readonly AppDbContext _context;

        public PropertyController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.Properties.ToList();

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var property =
                _context.Properties.Find(id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        public IActionResult Delete(int id)
        {
            var property = _context.Properties.Find(id);

            if (property == null)
            {
                return NotFound();
            }

            _context.Properties.Remove(property);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Create(Property property)
        {
            _context.Properties.Add(property);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Edit(Property property)
        {
            _context.Properties.Update(property);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }

}
