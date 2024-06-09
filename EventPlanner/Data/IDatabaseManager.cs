using EventPlanner.Data.AbstractClasses;
using EventPlanner.Data.DataClasses;

namespace EventPlanner.Data;

public interface IDatabaseManager
{
    // FestivalController methods
    Task<List<DataFestival>> ReadAllFestivals();
    Task<DataFestival> RequestFestivalByIdAsync(string festivalId);
    Task AddNewFestivalAsync(DataFestival festival);
    Task BoundRoomToFestival(string festivalId, string roomId);
    Task RemoveRoomFromFestival(string festivalId, string roomId);
    Task DeleteFestivalAsync(string festivalId);
    Task UpdateFestival(DataFestival updatedFestival);

    // SegmentController methods
    Task<List<Segment>> ReadAllSegments();
    Task<Segment> RequestSegmentByIdAsync(string segmentId);
    Task AddNewSegmentAsync(Segment segment);
    Task BoundParticipantToSegmentAsync(string segmentId, string userId);
    Task BoundParticipantToSegmentPositionAsync(string segmentId, string userId, int position);
    Task UpdateSegment(Segment updatedSegment);
    Task RemoveBoundedParticipantFromSegmentAsync(string segmentId, string participantId);
    Task RemoveBoundedParticipantFromSegmentPositionAsync(string segmentId, string participantId, int position);
    Task DeleteSegmentAsync(string segmentId);

    // RoomController methods
    Task<List<Room>> ReadAllRooms();
    Task<Room> RequestRoomByIdAsync(string roomId);
    Task AddNewRoomAsync(Room room);
    Task BoundSegmentToRoom(string roomId, string segmentId);
    Task UpdateRoom(Room updatedRoom);
    Task DeleteRoomAsync(string roomId);
    Task RemoveSegmentFromRoom(string roomId, string segmentId);

    // ParticipantController methods
    Task<List<DataParticipant>> ReadAllParticipants();
    Task<DataParticipant> RequestParticipantByIdAsync(string userId);
    Task<DataParticipant> RequestParticipantByEmailAsync(string email);
    Task AddNewParticipantAsync(DataParticipant participant);
    Task UpdateParticipant(DataParticipant updatedParticipant);
    Task DeleteParticipantAsync(string? email, string? userId);
}