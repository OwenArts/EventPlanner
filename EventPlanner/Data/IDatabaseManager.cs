using EventPlanner.Data.AbstractClasses;
using EventPlanner.Data.DataClasses;

namespace EventPlanner.Data;

public interface IDatabaseManager
{
    #region ParticipantController methods

    Task<List<Participant>> ReadAllParticipants();
    Task AddNewParticipantAsync(Participant participant);
    Task<Participant> RequestParticipantByIdAsync(string userId);
    Task<Participant> RequestParticipantByEmailAsync(string userEmail);
    Task DeleteParticipantAsync(string? email = null, string? userId = null);
    Task UpdateParticipant(Participant updatedParticipant);

    #endregion

    #region SegmentController methods

    Task<List<Segment>> ReadAllSegments();
    Task<Segment> RequestSegmentByIdAsync(string segmentId);
    Task<List<Segment>> RequestSegmentsByRoomIdAsync(string roomId);
    Task AddNewSegmentAsync(Segment segment);
    Task BoundParticipantToSegmentAsync(string segmentId, string userId);
    Task BoundParticipantToSegmentPositionAsync(string segmentId, string userId, int position);
    Task RemoveBoundedParticipantFromSegmentAsync(string segmentId, string userId);
    Task RemoveBoundedParticipantFromSegmentPositionAsync(string segmentId, string userId, int position);
    Task RemoveAllBoundedParticipantFromSegmentAsync(string segmentId);
    Task<List<Participant>> RequestBoundedParticipantToSegmentAsync(string segmentId);
    Task DeleteSegmentAsync(string segmentId);
    Task UpdateSegment(Segment updatedSegment);

    #endregion

    #region RoomController methods

    Task<List<DataRoom>> ReadAllRooms();
    Task AddNewRoomAsync(Room room);
    Task<Room> RequestRoomByIdAsync(string roomId);
    Task<List<Room>> RequestRoomByFestivalIdAsync(string festivalId);
    Task BoundSegmentToRoom(string roomId, string segmentId);
    Task RemoveSegmentFromRoom(string roomId, string segmentId);
    Task RemoveAllSegmentsFromRoom(string roomId);
    Task DeleteRoomAsync(string roomId);
    Task UpdateRoom(Room updatedRoom);

    #endregion

    #region FestivalController methods

    Task<List<DataFestival>> ReadAllFestivals();
    Task AddNewFestivalAsync(Festival festival);
    Task<DataFestival> RequestFestivalByIdAsync(string festivalId);
    Task BoundRoomToFestival(string festivalId, string roomId);
    Task RemoveRoomFromFestival(string festivalId, string roomId);
    Task RemoveAllRoomsFromFestival(string festivalId);
    Task DeleteFestivalAsync(string festivalId);
    Task UpdateFestival(Festival updatedFestival);

    #endregion
}