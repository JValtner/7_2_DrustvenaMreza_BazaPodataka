using _6_1_drustvena_mreza.DOMEN;
using Microsoft.Data.Sqlite;

namespace _7_2_drustvena_mreza.REPO
{
    public class UserDbRepo
    {
        public List<Korisnik> GetAll()
        {
            List<Korisnik> listaKorisnika = new List<Korisnik>();
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
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

                    DateTime DatumRodjenja = DateTime.ParseExact(reader["Birthday"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture
);
                    Korisnik k = new Korisnik(id, KorisnickoIme, Ime, Prezime, DatumRodjenja);
                    listaKorisnika.Add(k);
                }
                return listaKorisnika;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                return null;

            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                return null;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                return null;
            }

        }

        public Korisnik GetById(int userId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
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
                return null;

            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                return null;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                return null;
            }

        }

        public int Create(Korisnik noviKorisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
                connection.Open();

                string query = @"INSERT INTO USERS (Username, Name, Surname, Birthday) values (@KorisnickoIme, @Ime, @Prezime, @DatumRodjenja); SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                                
                command.Parameters.AddWithValue("@KorisnickoIme", noviKorisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@Ime", noviKorisnik.Ime);
                command.Parameters.AddWithValue("@Prezime", noviKorisnik.Prezime);
                command.Parameters.AddWithValue("@DatumRodjenja", noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd"));

                int lastInsertedId = Convert.ToInt32(command.ExecuteScalar());
                Console.WriteLine($"ID poslednjeg unetog korisnika: {lastInsertedId}");

                return lastInsertedId;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                return 0;

            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                return 0;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                return 0;
            }

        }

        public int Update(int id, Korisnik noviKorisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
                connection.Open();

                string query = "UPDATE USERS set Username = @KorisnickoIme, Name = @Ime, Surname = @Prezime, Birthday = @DatumRodjenja WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@KorisnickoIme", noviKorisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@Ime", noviKorisnik.Ime);
                command.Parameters.AddWithValue("@Prezime", noviKorisnik.Prezime);
                command.Parameters.AddWithValue("@DatumRodjenja", noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd"));

                return command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                return 0;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                return 0;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                return 0;
            }
        }

        public int Delete(int userId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
                connection.Open();

                string query = "DELETE FROM Users WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", userId);

                return command.ExecuteNonQuery();

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                return 0;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                return 0;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                return 0;
            }
        }
    }
}
