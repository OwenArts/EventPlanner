using EventPlanner.Data.AbstractClasses;

namespace EventPlanner.Data.DataClasses;

public class DataFestival : Festival
{
    public DataFestival(string name, DateTime startMoment, DateTime endMoment, string? id = null, List<Room>? rooms = null) : base(name, startMoment, endMoment, id, rooms)
    {
    }
}