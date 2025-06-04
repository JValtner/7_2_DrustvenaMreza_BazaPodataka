using _7_2_drustvena_mreza.DOMEN;
using Microsoft.Data.Sqlite;

namespace _7_2_drustvena_mreza.REPO
{
    public class UserDbRepo
    {

        private readonly string connectionString;

        private readonly GroupMembershipRepo groupMembershipRepo;

        public UserDbRepo(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
            groupMembershipRepo = new GroupMembershipRepo(configuration);
        }

        public List<Korisnik> GetAll()
        {
            List<Korisnik> listaKorisnika = new List<Korisnik>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string KorisnickoIme = reader["Username"].ToString();
                    string Ime = reader["Name"].ToString();
                    string Prezime = reader["Surname"].ToString();

                    DateTime DatumRodjenja = DateTime.ParseExact(reader["Birthday"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    List<Grupa> GrupeKorisnika = groupMembershipRepo.GetMemberships(id);
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

        public List<Korisnik> GetPaged(int page, int pageSize)
        {
            List<Korisnik> listaKorisnika = new List<Korisnik>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Users LIMIT @PageSize OFFSET @Offset";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string KorisnickoIme = reader["Username"].ToString();
                    string Ime = reader["Name"].ToString();
                    string Prezime = reader["Surname"].ToString();

                    DateTime DatumRodjenja = DateTime.ParseExact(reader["Birthday"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    List<Grupa> GrupeKorisnika = groupMembershipRepo.GetMemberships(id);
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

        public Korisnik GetById(int userId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Users where id= @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("Id", userId);
                using SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string KorisnickoIme = reader["Username"].ToString();
                    string Ime = reader["Name"].ToString();
                    string Prezime = reader["Surname"].ToString();

                    DateTime DatumRodjenja = DateTime.ParseExact(reader["Birthday"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture
);
                    Korisnik k = new Korisnik(id, KorisnickoIme, Ime, Prezime, DatumRodjenja);
                    return k;
                }
                return null;
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

        public Korisnik Create(Korisnik noviKorisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO USERS (Username, Name, Surname, Birthday) values (@KorisnickoIme, @Ime, @Prezime, @DatumRodjenja); SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                                
                command.Parameters.AddWithValue("@KorisnickoIme", noviKorisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@Ime", noviKorisnik.Ime);
                command.Parameters.AddWithValue("@Prezime", noviKorisnik.Prezime);
                command.Parameters.AddWithValue("@DatumRodjenja", noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd"));

                noviKorisnik.Id = Convert.ToInt32(command.ExecuteScalar());
                Console.WriteLine($"ID poslednjeg unetog korisnika: {noviKorisnik.Id}");

                return noviKorisnik;
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

        public Korisnik Update(Korisnik noviKorisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "UPDATE USERS set Username = @KorisnickoIme, Name = @Ime, Surname = @Prezime, Birthday = @DatumRodjenja WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", noviKorisnik.Id);
                command.Parameters.AddWithValue("@KorisnickoIme", noviKorisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@Ime", noviKorisnik.Ime);
                command.Parameters.AddWithValue("@Prezime", noviKorisnik.Prezime);
                command.Parameters.AddWithValue("@DatumRodjenja", noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd"));

                int affectedRows = command.ExecuteNonQuery();
                return affectedRows > 0 ? noviKorisnik : null;
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

        public bool Delete(int userId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Users WHERE Id=@Id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", userId);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;

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

        public int CountAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Count(*) AS Count FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);
                return Convert.ToInt32(command.ExecuteScalar());

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
