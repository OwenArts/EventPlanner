using EventPlanner.Data.AbstractClasses;

namespace EventPlanner.Data.TimetableClasses;

public class PlannerFestival : Festival
{
    public PlannerFestival(string name, DateTime startMoment, DateTime endMoment, string? id = null, List<Room>? rooms = null) : base(name, startMoment, endMoment, id, rooms)
    {
    }
}