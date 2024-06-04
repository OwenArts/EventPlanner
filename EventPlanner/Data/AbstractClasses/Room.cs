namespace EventPlanner.Data.AbstractClasses
{
    public abstract class Room
    {
        public Room(string name, DateTime timeOpen, DateTime timeClose, string? id = null,
            List<Segment>? segments = null)
        {
            this.id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            this.name = name;
            this.timeOpen = timeOpen;
            this.timeClose = timeClose;
            this.segments = segments == null ? new() : segments;
        }

        public string id { get; set; }
        public string name { get; set; }
        public DateTime timeOpen { get; set; }
        public DateTime timeClose { get; set; }
        public List<Segment>? segments { get; set; }
    }
}