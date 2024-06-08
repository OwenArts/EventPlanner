using EventPlanner.Data.AbstractClasses;

namespace EventPlanner.Data.DataClasses;

public class DataRoom : Room
{
    public DataRoom(string name, DateTime timeOpen, DateTime timeClose, string? id = null, List<Segment>? segments = null) : base(name, timeOpen, timeClose, id, segments)
    {
    }
}