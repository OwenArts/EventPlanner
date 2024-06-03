using EventPlanner.Data;
using Newtonsoft.Json;

namespace EventPlanner.Managers;

public class PlannerManager
{
    public PlannerManager()
    {
    }

    private static PlannerManager _instance;

    public static PlannerManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new();

            return _instance;
        }
        internal set { _instance = value; }
    }

    public void ValidateFestival(string jsonString) =>
        ValidateFestival(JsonConvert.DeserializeObject<Festival>(jsonString));
    
    public void ValidateFestival(Festival festival)
    {
        if (festival == null)
            throw new NullReferenceException("Festival is null.");

        // Validate festival's start and end moments
        if (festival.startMoment >= festival.endMoment)
            throw new ArgumentException("Festival start moment must be before end moment.");

        if(festival.rooms.Count<=0)
            throw new ArgumentException("Festival doesnt have any rooms.");

        // Validate rooms
        foreach (var room in festival.rooms)
            ValidateRoom(room, festival.startMoment, festival.endMoment);
    }

    private void ValidateRoom(Room room, DateTime festivalStart, DateTime festivalEnd)
    {
        // Check if room start moment is before end moment
        if (room.timeOpen >= room.timeClose)
            throw new ArgumentException($"Room '{room.name}' start moment must be before end moment.");

        // Check if room start moment is within festival start and end moments
        if (room.timeOpen < festivalStart || room.timeClose > festivalEnd)
            throw new ArgumentException($"Room '{room.name}' start and/or end moments do not align with the festival.");

        if(room.segments.Count<=0)
            throw new ArgumentException("Room doesnt have any segments.");
        // Check if the segments don't take up more time than the room has.
        ValidRoomSegments(room);
    }

    private void ValidRoomSegments(Room room)
    {
        var availableTime = (room.timeClose - room.timeOpen).TotalMinutes;

        double totalTime = 0.0;

        foreach (var segment in room.segments)
            totalTime += segment.duration * segment.contestants.Count;
        
        Console.WriteLine($"Available time: {availableTime} minutes and required amount of minutes is {totalTime}.");

        if (totalTime > availableTime && totalTime > 0)
            throw new ArgumentException($"Room '{room.name}' segments are too long for the available time. Available time: {availableTime} minutes and required amount of minutes is {totalTime}.");
    }

    public void PlanFestival(Festival festival)
    {
        /* Todo:
         * - Validate festival
         * - Foreach (room)
         *  - PlanRoom() Start planning from the end of the festival to the start of the festival
         */
    }

    private void PlanRoom(Room room, DateTime festivalEndMoment)
    {
        /* Todo:
         * - local var -> EndTime
         * - randomly sort list of segments
         * - foreach (segment)
         *      - PlanSegment()
         *      - this.EndTime - PlannedSegmentTimeTotal
         * - Check for overlap (could have)
         *      -if overlap, redo planning of all rooms
         * - return list of segments with a timeframe for each segment
         */
    }

    private void PlanSegment(Segment segment, DateTime roomEndMoment)
    {
        /* Todo:
         * - randomly sort list of contestants
         * - Foreach (contestant)
         *      - timetable+=contestant
         *      - contestant end moment = roomEndMoment
         *      - roomEndMoment.minutes-duration
         *      - contestant start moment = roomEndMoment 
         * - return list with all contestants of the segment and each timeframe
         */
    }
    
    /*
     public void PlanFestival(Festival festival)
{
    // Validate festival
    if (festival == null || festival.rooms == null || !festival.rooms.Any())
    {
        throw new ArgumentException("Invalid festival data.");
    }

    // Plan each room
    foreach (var room in festival.rooms)
    {
        PlanRoom(room, festival.endMoment);
    }
}

private void PlanRoom(Room room, DateTime festivalEndMoment)
{
    // Initialize end time
    DateTime endTime = festivalEndMoment;

    // Randomly sort list of segments
    var random = new Random();
    var segments = room.segments.OrderBy(x => random.Next()).ToList();

    // Plan each segment
    foreach (var segment in segments)
    {
        endTime = PlanSegment(segment, endTime);
        segment.endTime = endTime;
        segment.startTime = endTime.AddMinutes(-segment.duration);
    }

    // Check for overlap and redo planning if necessary
    // This part is left as an exercise for the reader
}

private DateTime PlanSegment(Segment segment, DateTime roomEndMoment)
{
    // Randomly sort list of contestants
    var random = new Random();
    var contestants = segment.contestants.OrderBy(x => random.Next()).ToList();

    // Plan each contestant
    foreach (var contestant in contestants)
    {
        // Update room end moment
        roomEndMoment = roomEndMoment.AddMinutes(-segment.duration);

        // Update contestant start and end moment
        contestant.startMoment = roomEndMoment;
        contestant.endMoment = roomEndMoment.AddMinutes(segment.duration);
    }

    // Return the updated room end moment
    return roomEndMoment;
}


private DateTime PlanSegment(Segment segment, DateTime roomEndMoment, Dictionary<string, DateTime> participantSchedules)
{
    // Randomly sort list of contestants
    var random = new Random();
    var contestants = segment.contestants.OrderBy(x => random.Next()).ToList();

    // Plan each contestant
    foreach (var contestant in contestants)
    {
        // Check if contestant is available
        if (participantSchedules.ContainsKey(contestant.id) && participantSchedules[contestant.id] > roomEndMoment)
        {
            throw new InvalidOperationException($"Contestant {contestant.id} is not available at {roomEndMoment}");
        }

        // Update room end moment
        roomEndMoment = roomEndMoment.AddMinutes(-segment.duration);

        // Update contestant start and end moment
        contestant.startMoment = roomEndMoment;
        contestant.endMoment = roomEndMoment.AddMinutes(segment.duration);

        // Update contestant's schedule
        participantSchedules[contestant.id] = contestant.endMoment;
    }

    // Return the updated room end moment
    return roomEndMoment;
}

     */
}