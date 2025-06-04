using _7_2_drustvena_mreza.DOMEN;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
namespace _7_2_drustvena_mreza.REPO
{
    public class GroupMembershipRepo
    {
        private readonly string connectionString;

        public GroupMembershipRepo(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }
        public List<Grupa> GetMemberships(int userId)
        {
            List<Grupa> listaGrupa = new List<Grupa>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT g.Id, g.Name, g.CreationDate FROM Groups g INNER JOIN GroupMemberships gm ON g.Id = gm.GroupId WHERE gm.UserId = @UserId";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string Ime = reader["Name"].ToString();
                    DateTime DatumOsnivanja = DateTime.ParseExact(reader["CreationDate"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    Grupa g = new Grupa(id, Ime, DatumOsnivanja);
                    listaGrupa.Add(g);
                }
                return listaGrupa;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;

            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }

        }
        public int NewMembership(int userId, int groupId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO GroupMemberships (UserId,GroupId) VALUES(@UserId,@GroupId)";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);

                return command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }

        }
        public int DeleteMembership(int userId, int groupId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM GroupMemberships WHERE UserId=@UserId AND GroupId = @GroupId";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);

                return command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }

        }
    }
}
