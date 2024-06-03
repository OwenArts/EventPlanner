using System.Security;

namespace EventPlanner.Data
{
    public record Festival
    {
        public Festival(string name, DateTime startMoment, DateTime endMoment, string? id = null, List<Room>? rooms = null)
        {
            this.id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            this.name = name;
            this.startMoment = startMoment;
            this.endMoment = endMoment;
            this.rooms = rooms == null ? new() : rooms;
        }

        public string id { get; set; }
        public string name { get; set; }
        public DateTime startMoment { get; set; }
        public DateTime endMoment { get; set; }
        public List<Room>? rooms { get; set; }
    }
}