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

        public IActionResult Details(int id)
        {
            var property = _context.Properties.Find(id);

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

            return View(property);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int PropertyId)
        {
            var property = _context.Properties.Find(PropertyId);

            if (property == null)
            {
                return NotFound();
            }

            var hasRooms = _context.Rooms.Any(r => r.PropertyId == PropertyId);

            if (hasRooms)
            {
                ModelState.AddModelError("", "この物件には部屋データが登録されているため削除できません。");
                return View(property);
            }

            _context.Properties.Remove(property);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }

}
