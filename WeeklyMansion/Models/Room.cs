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
        public string RoomNo { get; set; } = "";

        public string? RoomName { get; set; }

        public int Capacity { get; set; }

        public int Status { get; set; }

        [ForeignKey(nameof(PropertyId))]
        public Property? Property { get; set; }
    }
}
