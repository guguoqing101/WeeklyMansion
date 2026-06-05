using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeeklyMansion.Data;
using WeeklyMansion.Models;

namespace WeeklyMansion.Controllers
{
    public class CalendarController : Controller
    {
        private readonly AppDbContext _context;

        public CalendarController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? propertyId, int? roomId)
        {
            var properties = await _context.Properties
                .OrderBy(p => p.PropertyName)
                .ToListAsync();

            var selectedPropertyId = propertyId ?? properties.FirstOrDefault()?.PropertyId;

            var roomsQuery = _context.Rooms.AsQueryable();
            if (selectedPropertyId.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.PropertyId == selectedPropertyId.Value);
            }

            var rooms = await roomsQuery
                .Where(r => r.IsActive)
                .OrderBy(r => r.RoomNo)
                .ToListAsync();

            ViewBag.PropertyId = selectedPropertyId;
            ViewBag.RoomId = roomId;
            ViewBag.Properties = new SelectList(properties, "PropertyId", "PropertyName", selectedPropertyId);
            ViewBag.Rooms = rooms.Select(r => new SelectListItem
            {
                Value = r.RoomId.ToString(),
                Text = string.IsNullOrWhiteSpace(r.RoomName) ? r.RoomNo : $"{r.RoomNo} {r.RoomName}",
                Selected = roomId == r.RoomId
            }).ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Rooms(int propertyId)
        {
            var rooms = await _context.Rooms
                .Where(r => r.PropertyId == propertyId)
                .Where(r => r.IsActive)
                .OrderBy(r => r.RoomNo)
                .Select(r => new
                {
                    roomId = r.RoomId,
                    roomNo = r.RoomNo,
                    roomName = r.RoomName
                })
                .ToListAsync();

            return Json(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> Events(DateTime start, DateTime end, int? propertyId, int? roomId)
        {
            var roomsQuery = _context.Rooms
                .Where(r => r.IsActive)
                .AsQueryable();

            if (propertyId.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.PropertyId == propertyId.Value);
            }

            if (roomId.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.RoomId == roomId.Value);
            }

            var roomIds = await roomsQuery.Select(r => r.RoomId).ToListAsync();

            var reservations = (await _context.Reservations
                .Include(r => r.Room)
                .Where(r => roomIds.Contains(r.RoomId)
                    && r.CheckInDate < end
                    && start < r.CheckOutDate)
                .ToListAsync())
                .Select(r => new
                {
                    id = $"reservation-{r.ReservationId}",
                    title = $"{r.Room?.RoomNo} 予約済 {r.CustomerName}",
                    start = r.CheckInDate.ToString("yyyy-MM-dd"),
                    end = r.CheckOutDate.ToString("yyyy-MM-dd"),
                    allDay = true,
                    backgroundColor = "#2563eb",
                    borderColor = "#1d4ed8",
                    extendedProps = new
                    {
                        kind = "reservation",
                        roomId = r.RoomId,
                        reservationId = r.ReservationId,
                        customerName = r.CustomerName,
                        memo = r.Memo
                    }
                })
                .ToList();

            var roomStatuses = (await _context.RoomStatuses
                .Include(s => s.Room)
                .Where(s => roomIds.Contains(s.RoomId)
                    && s.TargetDate >= start
                    && s.TargetDate < end)
                .ToListAsync())
                .Select(s => new
                {
                    id = $"status-{s.RoomStatusId}",
                    title = $"{s.Room?.RoomNo} {RoomStatus.GetStatusName(s.Status)}",
                    start = s.TargetDate.ToString("yyyy-MM-dd"),
                    allDay = true,
                    backgroundColor = GetStatusColor(s.Status),
                    borderColor = GetStatusColor(s.Status),
                    extendedProps = new
                    {
                        kind = "status",
                        roomId = s.RoomId,
                        roomStatusId = s.RoomStatusId,
                        status = s.Status
                    }
                })
                .ToList();

            return Json(reservations.Cast<object>().Concat(roomStatuses));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRoomStatus(int roomId, DateTime targetDate, int status)
        {
            if (status < 0 || status > 3)
            {
                return BadRequest(new { message = "状態を選択してください。" });
            }

            var roomExists = await _context.Rooms.AnyAsync(r => r.RoomId == roomId && r.IsActive);
            if (!roomExists)
            {
                return NotFound(new { message = "部屋が見つからない、または利用停止中です。" });
            }

            var date = targetDate.Date;

            if (status is 2 or 3)
            {
                var hasReservation = await _context.Reservations.AnyAsync(r =>
                    r.RoomId == roomId &&
                    r.CheckInDate < date.AddDays(1) &&
                    date < r.CheckOutDate);

                if (hasReservation)
                {
                    return BadRequest(new { message = "予約期間中の日付は清掃中・メンテナンスに変更できません。" });
                }
            }

            var roomStatus = await _context.RoomStatuses
                .FirstOrDefaultAsync(s => s.RoomId == roomId && s.TargetDate == date);

            if (roomStatus == null)
            {
                roomStatus = new RoomStatus
                {
                    RoomId = roomId,
                    TargetDate = date,
                    Status = status
                };
                _context.RoomStatuses.Add(roomStatus);
            }
            else
            {
                roomStatus.Status = status;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "日別状態を保存しました。" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation(int roomId, DateTime checkInDate, DateTime checkOutDate, string customerName, string? memo)
        {
            customerName = customerName?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(customerName))
            {
                return BadRequest(new { message = "顧客名を入力してください。" });
            }

            var checkIn = checkInDate.Date;
            var checkOut = checkOutDate.Date;

            if (checkOut <= checkIn)
            {
                return BadRequest(new { message = "チェックアウト日はチェックイン日より後の日付にしてください。" });
            }

            var roomExists = await _context.Rooms.AnyAsync(r => r.RoomId == roomId && r.IsActive);
            if (!roomExists)
            {
                return NotFound(new { message = "部屋が見つからない、または利用停止中です。" });
            }

            var hasOverlappingReservation = await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                r.CheckInDate < checkOut &&
                checkIn < r.CheckOutDate);

            if (hasOverlappingReservation)
            {
                return BadRequest(new { message = "同じ部屋に期間が重なる予約があります。" });
            }

            var hasBlockedDate = await _context.RoomStatuses.AnyAsync(s =>
                s.RoomId == roomId &&
                s.TargetDate >= checkIn &&
                s.TargetDate < checkOut &&
                (s.Status == 2 || s.Status == 3));

            if (hasBlockedDate)
            {
                return BadRequest(new { message = "予約期間内に清掃中またはメンテナンスの日があります。" });
            }

            var reservation = new Reservation
            {
                RoomId = roomId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                CustomerName = customerName,
                Memo = memo
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "予約を登録しました。" });
        }

        private static string GetStatusColor(int status)
        {
            return status switch
            {
                0 => "#16a34a",
                1 => "#2563eb",
                2 => "#ca8a04",
                3 => "#dc2626",
                _ => "#6b7280"
            };
        }
    }
}
