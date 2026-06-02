
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
    public async Task<IActionResult> Details(int? roomid)
    {
        if (roomid == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms
            .FirstOrDefaultAsync(m => m.RoomId == roomid);
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
    public async Task<IActionResult> Create([Bind("RoomId,PropertyId,RoomNo,RoomName,Capacity,Status,Property")] Room room)
    {
        if (ModelState.IsValid)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(room);
    }

    // GET: ROOMS/Edit/5
    public async Task<IActionResult> Edit(int? roomid)
    {
        if (roomid == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms.FindAsync(roomid);
        if (room == null)
        {
            return NotFound();
        }
        return View(room);
    }

    // POST: ROOMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? roomid, [Bind("RoomId,PropertyId,RoomNo,RoomName,Capacity,Status,Property")] Room room)
    {
        if (roomid != room.RoomId)
        {
            return NotFound();
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
            return RedirectToAction(nameof(Index));
        }
        return View(room);
    }

    // GET: ROOMS/Delete/5
    public async Task<IActionResult> Delete(int? roomid)
    {
        if (roomid == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms
            .FirstOrDefaultAsync(m => m.RoomId == roomid);
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    // POST: ROOMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? roomid)
    {
        var room = await _context.Rooms.FindAsync(roomid);
        if (room != null)
        {
            _context.Rooms.Remove(room);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RoomExists(int? roomid)
    {
        return _context.Rooms.Any(e => e.RoomId == roomid);
    }
}
