using EventPlanner.Data.AbstractClasses;

namespace EventPlanner.Data.DataClasses;

public class DataParticipant : Participant
{
    public DataParticipant(string firstName, string lastName, string email, string? id = null, string? middleName = null, DateTime? birthDay = null, string? phoneNumber = null) : base(firstName, lastName, email, id, middleName, birthDay, phoneNumber)
    {
    }
}