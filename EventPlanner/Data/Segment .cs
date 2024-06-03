using Newtonsoft.Json;

namespace EventPlanner.Data
{
    public record Segment
    {
        public Segment(string name, int duration, string? id = null, Participant? firstPlace = null,
            Participant? secondPlace = null, Participant? thirdPlace = null, List<Participant>? contestants = null)
        {
            this.id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            this.name = name;
            this.contestants = contestants != null ? contestants : new();
            this.duration = duration;
            this.firstPlace = firstPlace;
            this.secondPlace = secondPlace;
            this.thirdPlace = thirdPlace;
        }

        public string id { get; set; }
        public string name { get; set; }
        public int duration { get; set; }
        public Participant firstPlace { get; set; }
        public Participant secondPlace { get; set; }
        public Participant thirdPlace { get; set; }
        public List<Participant>? contestants { get; set; }
    }
}