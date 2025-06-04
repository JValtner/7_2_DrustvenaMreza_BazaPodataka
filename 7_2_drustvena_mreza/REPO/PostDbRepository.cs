using _7_2_drustvena_mreza.DOMEN;
using Microsoft.Data.Sqlite;

namespace _7_2_drustvena_mreza.REPO
{
    public class PostDbRepository
    {
        private readonly string connectionString;

        public PostDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }
                
        public List<Post> GetAll()
        {
            List<Post> allPosts = new List<Post>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                var command = new SqliteCommand(@"
                    SELECT * 
                    FROM Posts p 
                    INNER JOIN Users u on p.UserId = u.id", connection);
                               
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Post p = new Post(
                    Convert.ToInt32(reader["Id"]),
                    Convert.ToInt32(reader["UserId"]),
                    reader["Content"].ToString(),
                    DateTime.ParseExact(reader["Date"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                    );
                    if (reader["UserId"] != DBNull.Value)
                    {
                        p.User = new Korisnik(
                            
                            Convert.ToInt32(reader["Id"]),
                            Convert.ToString(reader["Username"]),
                            Convert.ToString(reader["Name"]),
                            Convert.ToString(reader["Surname"]),
                            DateTime.ParseExact(reader["Birthday"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                        );
                    }
                    allPosts.Add(p);
                }
                return allPosts;
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
