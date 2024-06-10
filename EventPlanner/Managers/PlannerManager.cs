using EventPlanner.Data.AbstractClasses;
using EventPlanner.Data.DataClasses;
using EventPlanner.Data.TimetableClasses;
using Newtonsoft.Json;

namespace EventPlanner.Managers
{
    public class PlannerManager
    {
        private static readonly Lazy<PlannerManager> _instance = new(() => new PlannerManager());
        public static PlannerManager Instance => _instance.Value;

        private PlannerManager() { }

        public void ValidateFestival(string jsonString)
        {
            var festival = JsonConvert.DeserializeObject<DataFestival>(jsonString);
            ValidateFestival(festival);
        }

        public void ValidateFestival(Festival festival)
        {
            if (festival == null)
                throw new NullReferenceException("Festival is null.");

            if (festival.startMoment >= festival.endMoment)
                throw new ArgumentException("Festival start moment must be before end moment.");

            if (festival.rooms.Count <= 0)
                throw new ArgumentException("Festival doesn't have any rooms.");

            foreach (var room in festival.rooms)
                ValidateRoom(room, festival.startMoment, festival.endMoment);
        }

        private void ValidateRoom(Room room, DateTime festivalStart, DateTime festivalEnd)
        {
            if (room.timeOpen >= room.timeClose)
                throw new ArgumentException($"Room '{room.name}' start moment must be before end moment.");

            if (room.timeOpen < festivalStart || room.timeClose > festivalEnd)
                throw new ArgumentException($"Room '{room.name}' start and/or end moments do not align with the festival.");

            if (room.segments.Count <= 0)
                throw new ArgumentException("Room doesn't have any segments.");
            
            ValidateRoomSegments(room);
        }

        private void ValidateRoomSegments(Room room)
        {
            var availableTime = (room.timeClose - room.timeOpen).TotalMinutes;

            double totalTime = 0.0;
            foreach (var segment in room.segments)
                totalTime += segment.duration * segment.contestants.Count;

            if (totalTime > availableTime && totalTime > 0)
                throw new ArgumentException($"Room '{room.name}' segments are too long for the available time. Available time: {availableTime} minutes and required amount of minutes is {totalTime}.");
        }

        public PlannerFestival PlanFestival(Festival festival)
        {
            ValidateFestival(festival);

            var result = new PlannerFestival(festival.name, festival.startMoment, festival.endMoment, festival.id, new List<Room>());

            foreach (var room in festival.rooms)
                result.rooms.Add(PlanRoom(room));

            return result;
        }

        private PlannerRoom PlanRoom(Room room)
        {
            DateTime roomEndTime = room.timeClose;
            Shuffle(room.segments);

            var result = new PlannerRoom(room.name, room.timeOpen, room.timeClose, room.id, new List<Segment>());

            foreach (var segment in room.segments)
            {
                PlanSegment(segment, roomEndTime, out PlannerSegment plannedSegment, out DateTime newRoomEndTime);
                roomEndTime = newRoomEndTime;
                result.segments.Add(plannedSegment);
            }

            return result;
        }

        private void PlanSegment(Segment segment, DateTime roomEndMoment, out PlannerSegment segmentResult, out DateTime remainingRoomEndTime)
        {
            Shuffle(segment.contestants);

            var result = new PlannerSegment(roomEndMoment, segment.name, segment.duration, segment.id, segment.firstPlace, segment.secondPlace, segment.thirdPlace, new List<Participant>());

            foreach (var participant in segment.contestants)
            {
                var endTime = roomEndMoment;
                roomEndMoment = roomEndMoment.AddMinutes(-segment.duration);
                var startTime = roomEndMoment;

                var newlyPlannedParticipant = new PlannerParticipant(startTime, endTime, participant.firstName, participant.lastName, participant.email, participant.id, participant.middleName, participant.birthDay, participant.phoneNumber);

                result.contestants.Add(newlyPlannedParticipant);
            }

            result.TimeSlotEnd = roomEndMoment;

            // "return" values
            segmentResult = result;
            remainingRoomEndTime = roomEndMoment;
        }

        public void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
