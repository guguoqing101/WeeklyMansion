using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyMansion.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        [StringLength(20)]
        public string RoomNo { get; set; } = "";

        [StringLength(100)]
        public string? RoomName { get; set; }

        [Range(1, 10, ErrorMessage = "定員は1から10までで入力してください。")]
        public int Capacity { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        [NotMapped]
        public string IsActiveName => IsActive ? "利用可能" : "利用停止";

        [ForeignKey(nameof(PropertyId))]
        public Property? Property { get; set; }

        public ICollection<RoomStatus>? RoomStatuses { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
    }
}
