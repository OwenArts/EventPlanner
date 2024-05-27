using EventPlanner.Data;
using MySql.Data.MySqlClient;

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
        internal set
        {
            _instance = value;
        }
    }

    public DatabaseManager()
    {
        
    }

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
        string sqlQuery = "INSERT INTO participant (id, firstName, middleName, lastName, birthDay, email, phoneNumber) " +
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

    public void RequestParticipantAsync(string userId)
    {
        throw new NotImplementedException();
    }
}