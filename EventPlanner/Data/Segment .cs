namespace EventPlanner.Data
{
    public record Segment
    {
        public Segment(string name, string? id = null)
        {
            this.id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            this.name = name;
            this.contestants = new();
        }

        public string id { get; set; }
        public string name { get; set; }
        public Participant firstPlace { get; set; }
        public Participant secondPlace { get; set; }
        public Participant thirdPlace { get; set; }
        public List<Participant> contestants { get; set; }
    }
}
