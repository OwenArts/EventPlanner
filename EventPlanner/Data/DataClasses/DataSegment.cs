using EventPlanner.Data.AbstractClasses;

namespace EventPlanner.Data.DataClasses;

public class DataSegment : Segment
{
    public DataSegment(string name, int duration, string? id = null, Participant? firstPlace = null, Participant? secondPlace = null, Participant? thirdPlace = null, List<Participant>? contestants = null) : base(name, duration, id, firstPlace, secondPlace, thirdPlace, contestants)
    {
    }
}