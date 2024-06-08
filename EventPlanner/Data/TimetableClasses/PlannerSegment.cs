using EventPlanner.Data.AbstractClasses;

namespace EventPlanner.Data.TimetableClasses;

public class PlannerSegment : Segment
{
    public DateTime TimeSlotStart { get; set; }
    public DateTime? TimeSlotEnd { get; set; }
    public PlannerSegment(DateTime timeSlotStart, string name, int duration, string? id = null,
        Participant? firstPlace = null, Participant? secondPlace = null, Participant? thirdPlace = null,
        List<Participant>? contestants = null, DateTime? timeSlotEnd = null) : base(name, duration, id, firstPlace, secondPlace, thirdPlace, contestants)
    {
        TimeSlotStart = timeSlotStart;
        TimeSlotEnd = timeSlotEnd;
    }
}