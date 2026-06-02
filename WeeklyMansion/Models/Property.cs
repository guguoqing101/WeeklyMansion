namespace WeeklyMansion.Models
{
    public class Property
    {
        public int PropertyId { get; set; }

        public string PropertyName { get; set; } = "";

        public string Address { get; set; } = "";
        public ICollection<Room>? Rooms { get; set; }
    }
}
