namespace WeeklyMansion.Models
{
    public class RoomStatus
    {
        public long RoomStatusId { get; set; }

        public int RoomId { get; set; }

        public DateTime TargetDate { get; set; }

        public int Status { get; set; }
    }
}
