using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyMansion.Models
{
    public class Reservation
    {
        public long ReservationId { get; set; }

        public int RoomId { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = "";

        [StringLength(500)]
        public string? Memo { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room? Room { get; set; }
    }
}
