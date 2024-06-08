using EventPlanner.Data.AbstractClasses;

namespace EventPlanner.Data.TimetableClasses;

public class PlannerRoom : Room
{
    public PlannerRoom(string name, DateTime timeOpen, DateTime timeClose, string? id = null, List<Segment>? segments = null) : base(name, timeOpen, timeClose, id, segments)
    {
    }
}