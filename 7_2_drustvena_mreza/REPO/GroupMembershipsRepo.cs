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
        public List<Korisnik> GetGroupMemberships(int groupId)
        {
            List<Korisnik> listaKorisnika = new List<Korisnik>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT u.Id, u.Username, u.Name, u.Surname,u.Birthday FROM Users u INNER JOIN GroupMemberships gm ON u.Id = gm.UserId INNER JOIN Groups g ON g.Id = gm.GroupId WHERE g.Id = @GroupId";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@GroupId", groupId);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string KorisnickoIme = reader["Username"].ToString();
                    string Ime = reader["Name"].ToString();
                    string Prezime = reader["Surname"].ToString();

                    DateTime DatumRodjenja = DateTime.ParseExact(reader["Birthday"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    List<Grupa> GrupeKorisnika = GetMemberships(id);
                    Korisnik k = new Korisnik(id, KorisnickoIme, Ime, Prezime, DatumRodjenja);
                    k.GrupeKorisnika = GrupeKorisnika;
                    listaKorisnika.Add(k);
                }
                return listaKorisnika;
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
