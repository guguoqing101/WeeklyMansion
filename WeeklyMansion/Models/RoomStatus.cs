using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyMansion.Models
{
    public class RoomStatus
    {
        public long RoomStatusId { get; set; }

        public int RoomId { get; set; }

        public DateTime TargetDate { get; set; }

        public int Status { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room? Room { get; set; }

        public static string GetStatusName(int status)
        {
            return status switch
            {
                0 => "空室",
                1 => "予約済",
                2 => "清掃中",
                3 => "メンテナンス",
                _ => "不明"
            };
        }
    }
}
