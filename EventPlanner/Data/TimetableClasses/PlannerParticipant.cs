using EventPlanner.Data.AbstractClasses;

namespace EventPlanner.Data.TimetableClasses;

public class PlannerParticipant : Participant
{
    public DateTime TimeSlotStart { get; set; }
    public DateTime TimeSlotEnd { get; set; }
    
    public PlannerParticipant(DateTime timeSlotStart, DateTime timeSlotEnd,string firstName, string lastName, string email, string? id = null, string? middleName = null, DateTime? birthDay = null, string? phoneNumber = null) 
        : base(firstName, lastName, email, id, middleName, birthDay, phoneNumber)
    {
        TimeSlotStart = timeSlotStart;
        TimeSlotEnd = timeSlotEnd;
    }
}