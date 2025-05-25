using _6_1_drustvena_mreza.DOMEN;
using Microsoft.Data.Sqlite;

namespace _7_2_drustvena_mreza.REPO
{
    public class UserDbRepository
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
    }
}
