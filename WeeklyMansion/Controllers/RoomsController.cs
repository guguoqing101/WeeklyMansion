
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeeklyMansion.Models;
using WeeklyMansion.Data;

public class RoomsController : Controller
{
    private readonly AppDbContext _context;

    public RoomsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: ROOMS
    public async Task<IActionResult> Index(int? propertyId)    
    {
        ViewBag.PropertyId = propertyId;

        var rooms = _context.Rooms.AsQueryable();

        if (propertyId.HasValue)
        {
            rooms = rooms.Where(r => r.PropertyId == propertyId.Value);
        }

        return View(await rooms.ToListAsync());
    }

    // GET: ROOMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms
            .FirstOrDefaultAsync(m => m.RoomId == id);
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    // GET: ROOMS/Create
    public IActionResult Create(int propertyId)
    {
        var room = new Room
        {
            PropertyId = propertyId
        };

        return View(room);
    }

    // POST: ROOMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RoomId,PropertyId,RoomNo,RoomName,Capacity,IsActive,Property")] Room room)
    {
        room.RoomNo = room.RoomNo.Trim();

        if (await RoomNoExistsAsync(room.PropertyId, room.RoomNo))
        {
            ModelState.AddModelError(nameof(Room.RoomNo), "Ç±ÇÃï®åèÇ…ÇÕìØÇ∂ïîâÆNoÇ™ä˘Ç…Ç†ÇËÇ‹Ç∑ÅB");
        }

        if (ModelState.IsValid)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { propertyId = room.PropertyId });
        }
        return View(room);
    }

    // GET: ROOMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }
        return View(room);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("RoomId,PropertyId,RoomNo,RoomName,Capacity,IsActive,Property")] Room room)
    {
        if (id != room.RoomId)
        {
            return NotFound();
        }

        room.RoomNo = room.RoomNo.Trim();

        if (await RoomNoExistsAsync(room.PropertyId, room.RoomNo, room.RoomId))
        {
            ModelState.AddModelError(nameof(Room.RoomNo), "Ç±ÇÃï®åèÇ…ÇÕìØÇ∂ïîâÆNoÇ™ä˘Ç…Ç†ÇËÇ‹Ç∑ÅB");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(room);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.RoomId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index), new { propertyId = room.PropertyId });
        }

        return View(room);
    }

    // GET: ROOMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms
            .FirstOrDefaultAsync(m => m.RoomId == id);
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room != null)
        {
            var propertyId = room.PropertyId;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { propertyId = propertyId });
        }

        return RedirectToAction(nameof(Index));
    }

    private bool RoomExists(int? id)
    {
        return _context.Rooms.Any(e => e.RoomId == id);
    }

    private Task<bool> RoomNoExistsAsync(int propertyId, string roomNo, int? excludeRoomId = null)
    {
        return _context.Rooms.AnyAsync(r =>
            r.PropertyId == propertyId &&
            r.RoomNo == roomNo &&
            (!excludeRoomId.HasValue || r.RoomId != excludeRoomId.Value));
    }
}
