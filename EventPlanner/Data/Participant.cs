namespace EventPlanner.Data
{
    public record Participant
    {
        public Participant(string firstName, string lastName, string email, string? id = null, string? middleName = null, DateTime? birthDay = null, string? phoneNumber = null)
        {
            this.id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            this.firstName = firstName;
            this.middleName = string.IsNullOrEmpty(middleName) ? string.Empty : middleName;
            this.lastName = lastName;
            this.birthDay = birthDay;
            this.email = email;
            this.phoneNumber = phoneNumber;
        }

        public string id { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public DateTime? birthDay { get; set; }
        public string email { get; set; }
        public string? phoneNumber { get; set; }
    }
}
