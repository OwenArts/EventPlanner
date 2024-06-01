using System.Data;
using EventPlanner.Data;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

public class DatabaseManager
{
    private string dbS = "localhost";
    private string dbI = "api";
    private string dbP = "M4KTg/1*YX5zBJ7E";
    private string dbD = "eventplanner_schema";

    private static DatabaseManager _instance;

    public static DatabaseManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new();

            return _instance;
        }
        internal set { _instance = value; }
    }

    public DatabaseManager()
    {
    }

    #region Participant

    public List<Participant> ReadAllParticipants()
    {
        var result = new List<Participant>();

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = dbS;
            builder.UserID = dbI;
            builder.Password = dbP;
            builder.Database = dbD;

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                string query = "SELECT * FROM participant";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (MySqlException sqlException)
                    {
                        Console.WriteLine($"Couldm't open connection to database.\n\r{sqlException}");
                        return null;
                    }

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Participant(
                                reader.GetString("firstName"),
                                reader.GetString("lastName"),
                                reader.GetString("email"),
                                reader.GetGuid("id").ToString(),
                                reader.GetString("middleName"),
                                reader.GetDateTime("birthDay"),
                                reader.GetString("phoneNumber")
                            ));
                        }
                    }
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Error: " + e.ToString());
        }

        return result;
    }

    public async Task AddNewParticipantAsync(Participant participant)
    {
        string sqlQuery =
            "INSERT INTO participant (id, firstName, middleName, lastName, birthDay, email, phoneNumber) " +
            "VALUES (@Id, @FirstName, @MiddleName, @LastName, @BirthDay, @Email, @PhoneNumber)";

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = dbS;
            builder.UserID = dbI;
            builder.Password = dbP;
            builder.Database = dbD;

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", participant.id);
                    command.Parameters.AddWithValue("@FirstName", participant.firstName);
                    command.Parameters.AddWithValue("@MiddleName", participant.middleName);
                    command.Parameters.AddWithValue("@LastName", participant.lastName);
                    command.Parameters.AddWithValue("@BirthDay", participant.birthDay);
                    command.Parameters.AddWithValue("@Email", participant.email);
                    command.Parameters.AddWithValue("@PhoneNumber", participant.phoneNumber);

                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Error: " + e);
        }
    }

    public async Task<Participant> RequestParticipantByEmailAsync(string userEmail)
    {
        var query = "SELECT * FROM `participant` WHERE `participant`.`email` = @Email";
        return await RequestParticipantByQueryAsync(query, new MySqlParameter("@Email", userEmail));
    }

    public async Task<Participant> RequestParticipantByIdAsync(string userId)
    {
        var query = "SELECT * FROM `participant` WHERE `participant`.`id` = @Id";
        return await RequestParticipantByQueryAsync(query, new MySqlParameter("@Id", userId));
    }

    private async Task<Participant> RequestParticipantByQueryAsync(string sqlQuery, params MySqlParameter[] parameters)
    {
        Participant? r = null;

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = dbS,
                UserID = dbI,
                Password = dbP,
                Database = dbD
            };

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddRange(parameters);

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                r = new Participant(
                                    reader.GetString("firstName"),
                                    reader.GetString("lastName"),
                                    reader.GetString("email"),
                                    reader.GetGuid("id").ToString(),
                                    reader.IsDBNull(reader.GetOrdinal("middleName"))
                                        ? string.Empty
                                        : reader.GetString("middleName"),
                                    reader.IsDBNull(reader.GetOrdinal("birthDay"))
                                        ? (DateTime?)null
                                        : reader.GetDateTime("birthDay"),
                                    reader.IsDBNull(reader.GetOrdinal("phoneNumber"))
                                        ? string.Empty
                                        : reader.GetString("phoneNumber")
                                );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Error: " + e);
        }

        return r!;
    }

    public async void DeleteParticipantAsync(string? email = null, string? userId = null)
    {
        if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(userId))
            return;

        var sqlQuery = "DELETE FROM `participant` WHERE ";

        if (!string.IsNullOrEmpty(email))
            sqlQuery += "`email` = @Email";
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(userId))
            sqlQuery += " AND ";
        if (!string.IsNullOrEmpty(userId))
            sqlQuery += "`id` = @Id";

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = dbS,
                UserID = dbI,
                Password = dbP,
                Database = dbD
            };

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    if (!string.IsNullOrEmpty(email))
                        command.Parameters.AddWithValue("@Email", email);
                    if (!string.IsNullOrEmpty(userId))
                        command.Parameters.AddWithValue("@Id", userId);

                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"{rowsAffected} row(s) deleted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error during command execution: " + ex.Message);
                    }
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Database error: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("General error: " + e.Message);
        }
    }

    public async void UpdateParticipant(Participant updatedParticipant)
    {
        if (updatedParticipant == null || string.IsNullOrEmpty(updatedParticipant.id))
        {
            Console.WriteLine("Invalid participant data.");
            return;
        }

        var checkQuery = "SELECT COUNT(*) FROM `participant` WHERE `id` = @Id";
        var updateQuery = @"UPDATE `participant` SET 
                        `firstName` = @FirstName, 
                        `middleName` = @MiddleName, 
                        `lastName` = @LastName, 
                        `birthDay` = @BirthDay, 
                        `email` = @Email, 
                        `phoneNumber` = @PhoneNumber 
                        WHERE `id` = @Id";

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = dbS,
                UserID = dbI,
                Password = dbP,
                Database = dbD
            };

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                await connection.OpenAsync();

                // Check if participant with given ID exists
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Id", updatedParticipant.id);
                    var exists = Convert.ToInt32(await checkCommand.ExecuteScalarAsync()) > 0;

                    if (!exists)
                    {
                        Console.WriteLine("Participant with given ID does not exist.");
                        return;
                    }
                }

                // Update participant details
                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@FirstName", updatedParticipant.firstName);
                    updateCommand.Parameters.AddWithValue("@MiddleName",
                        string.IsNullOrEmpty(updatedParticipant.middleName)
                            ? DBNull.Value
                            : (object)updatedParticipant.middleName);
                    updateCommand.Parameters.AddWithValue("@LastName", updatedParticipant.lastName);
                    updateCommand.Parameters.AddWithValue("@BirthDay", updatedParticipant.birthDay);
                    updateCommand.Parameters.AddWithValue("@Email", updatedParticipant.email);
                    updateCommand.Parameters.AddWithValue("@PhoneNumber", updatedParticipant.phoneNumber);
                    updateCommand.Parameters.AddWithValue("@Id", updatedParticipant.id);

                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Database error: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("General error: " + e.Message);
        }
    }

    #endregion

    #region Segment

    public IEnumerable<Segment> ReadAllSegments()
    {
        var result = new List<Segment>();
        var resultTopThreePlaces = new List<List<string>>();

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = dbS;
            builder.UserID = dbI;
            builder.Password = dbP;
            builder.Database = dbD;

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                string query = "SELECT * FROM `segment`";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (MySqlException sqlException)
                    {
                        Console.WriteLine($"Couldn't open connection to database.\n\r{sqlException}");
                        return null;
                    }

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Segment(
                                reader.GetString("name"),
                                int.Parse(reader.GetInt32("duration").ToString()),
                                reader.GetGuid("id").ToString()
                            ));

                            resultTopThreePlaces.Add(new List<string>()
                            {
                                reader.IsDBNull(reader.GetOrdinal("firstPlace"))
                                    ? string.Empty
                                    : reader.GetGuid("firstPlace").ToString(),
                                reader.IsDBNull(reader.GetOrdinal("secondPlace"))
                                    ? string.Empty
                                    : reader.GetGuid("secondPlace").ToString(),
                                reader.IsDBNull(reader.GetOrdinal("thirdPlace"))
                                    ? string.Empty
                                    : reader.GetGuid("thirdPlace").ToString(),
                            });
                        }
                    }
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < resultTopThreePlaces[i].Count; j++)
                {
                    switch (j)
                    {
                        case 0:
                            result[i].firstPlace = RequestParticipantByIdAsync(resultTopThreePlaces[i][j]).Result;
                            break;
                        case 1:
                            result[i].secondPlace = RequestParticipantByIdAsync(resultTopThreePlaces[i][j]).Result;
                            break;
                        case 2:
                            result[i].thirdPlace = RequestParticipantByIdAsync(resultTopThreePlaces[i][j]).Result;
                            break;
                    }
                }

                result[i].contestants = RequestBoundedParticipantToSegmentAsync(result[i].id).Result;
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Error: " + e.ToString());
        }

        return result;
    }

    public async Task<Segment> RequestSegmentByIdAsync(string segmentId)
    {
        Segment result = new Segment(string.Empty, 0);
        var resultTopThreePlaces = new List<string>();

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = dbS;
            builder.UserID = dbI;
            builder.Password = dbP;
            builder.Database = dbD;

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                string query = "SELECT * FROM `segment` where `segment`.`id` = @SegmentId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SegmentId", segmentId);

                    try
                    {
                        connection.Open();
                    }
                    catch (MySqlException sqlException)
                    {
                        Console.WriteLine($"Couldn't open connection to database.\n\r{sqlException}");
                        return null;
                    }

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = new Segment(
                                reader.GetString("name"),
                                int.Parse(reader.GetInt32("duration").ToString()),
                                reader.GetGuid("id").ToString()
                            );

                            resultTopThreePlaces.Add(reader.IsDBNull(reader.GetOrdinal("firstPlace"))
                                ? string.Empty
                                : reader.GetGuid("firstPlace").ToString());
                            resultTopThreePlaces.Add(reader.IsDBNull(reader.GetOrdinal("secondPlace"))
                                ? string.Empty
                                : reader.GetGuid("secondPlace").ToString());
                            resultTopThreePlaces.Add(reader.IsDBNull(reader.GetOrdinal("thirdPlace"))
                                ? string.Empty
                                : reader.GetGuid("thirdPlace").ToString());
                        }
                    }
                }

                for (int i = 0; i < resultTopThreePlaces.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            result.firstPlace = RequestParticipantByIdAsync(resultTopThreePlaces[i]).Result;
                            break;
                        case 1:
                            result.secondPlace = RequestParticipantByIdAsync(resultTopThreePlaces[i]).Result;
                            break;
                        case 2:
                            result.thirdPlace = RequestParticipantByIdAsync(resultTopThreePlaces[i]).Result;
                            break;
                    }
                }

                result.contestants = RequestBoundedParticipantToSegmentAsync(result.id).Result;
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Error: " + e);
        }

        return result != null ? result : null;
    }

    public async Task AddNewSegmentAsync(Segment segment)
    {
        string sqlQuery =
            "INSERT INTO segment (id, name, roomId, firstPlace, secondPlace, thirdPlace, duration) " +
            "VALUES (@Id, @Name, @RoomId, @FirstPlace, @SecondPlace, @ThirdPlace, @Duration)";

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = dbS;
            builder.UserID = dbI;
            builder.Password = dbP;
            builder.Database = dbD;

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", segment.id);
                    command.Parameters.AddWithValue("@Name", segment.name);
                    command.Parameters.AddWithValue("@RoomId", null); // Assuming segment has a roomId property
                    command.Parameters.AddWithValue("@FirstPlace",
                        segment.firstPlace != null ? segment.firstPlace.id : null);
                    command.Parameters.AddWithValue("@SecondPlace",
                        segment.secondPlace != null ? segment.secondPlace.id : null);
                    command.Parameters.AddWithValue("@ThirdPlace",
                        segment.thirdPlace != null ? segment.thirdPlace.id : null);
                    command.Parameters.AddWithValue("@Duration", segment.duration);

                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Error: " + e);
        }
    }

    public async Task BoundParticipantToSegmentAsync(string segmentId, string userId)
    {
        string sqlQuery =
            "INSERT INTO segment_participant (segmentId, participantId) " +
            "VALUES (@SegmentId, @ParticipantId)";

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = dbS;
            builder.UserID = dbI;
            builder.Password = dbP;
            builder.Database = dbD;

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@SegmentId", segmentId);
                    command.Parameters.AddWithValue("@ParticipantId", userId);

                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Error: " + e);
        }
    }
    public async Task RemoveBoundedParticipantFromSegmentAsync(string segmentId, string userId)
    {
        string sqlQuery = "DELETE FROM `segment_participant` WHERE `segment_participant`.`segmentId` = @SegmentId AND `segment_participant`.`participantId` = @UserId";

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = dbS;
            builder.UserID = dbI;
            builder.Password = dbP;
            builder.Database = dbD;

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@SegmentId", segmentId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"{rowsAffected} row(s) deleted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Error: " + e);
        }
    }

    public async Task<List<Participant>> RequestBoundedParticipantToSegmentAsync(string segmentId)
    {
        var resultIds = new List<string>();
        var result = new List<Participant>();

        string sqlQuery =
            "SELECT * FROM `segment_participant` WHERE `segment_participant`.`segmentId` = @SegmentId";

        try
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = dbS;
            builder.UserID = dbI;
            builder.Password = dbP;
            builder.Database = dbD;

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@SegmentId", segmentId);

                    try
                    {
                        connection.Open();
                    }
                    catch (MySqlException sqlException)
                    {
                        Console.WriteLine($"Couldn't open connection to database.\n\r{sqlException}");
                        return null;
                    }

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            resultIds.Add(reader.GetGuid("participantId").ToString());
                    }
                }
            }

            foreach (string id in resultIds)
                result.Add(RequestParticipantByIdAsync(id).Result);
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Error: " + e);
        }

        return result;
    }

    #endregion
}