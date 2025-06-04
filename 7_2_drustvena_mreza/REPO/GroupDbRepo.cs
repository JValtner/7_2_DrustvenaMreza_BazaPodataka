using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using _7_2_drustvena_mreza.DOMEN;
using Microsoft.Data.Sqlite;
namespace _7_2_drustvena_mreza.REPO
{
    public class GroupDbRepo
    {
        private readonly string connectionString;

        public GroupDbRepo(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }
        public List<Grupa> GetPaged(int page, int pageSize)
        {
            List<Grupa> listaGrupa = new List<Grupa>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups LIMIT @PageSize OFFSET @Offset";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string ime = reader["Name"].ToString();
                    DateTime datumOsnivanja = DateTime.ParseExact(reader["CreationDate"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    Grupa g = new Grupa(id, ime, datumOsnivanja);

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
        public Grupa GetGroup(int grupaId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", grupaId);
                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string ime = reader["Name"].ToString();
                    DateTime datumOsnivanja = DateTime.ParseExact(reader["CreationDate"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    Grupa grupa = new Grupa(id, ime, datumOsnivanja);
                    return grupa;
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
        public int NewGroup(Grupa grupa)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO Groups (Name,CreationDate) VALUES (@Ime, @DatumOsnivanja)";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Ime", grupa.Ime);
                command.Parameters.AddWithValue("@DatumOsnivanja", grupa.DatumOsnivanja.ToString("yyyy-MM-dd"));
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
        public int UpdateGroup(int grupaId,Grupa grupa)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "UPDATE Groups SET Name = @Ime, CreationDate = @DatumOsnivanja WHERE Id=@Id ";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", grupa.Id);
                command.Parameters.AddWithValue("@Ime", grupa.Ime);
                command.Parameters.AddWithValue("@DatumOsnivanja", grupa.DatumOsnivanja.ToString("yyyy-MM-dd"));
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

        public int DeleteGroup(int grupaId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Groups WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id",grupaId);
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
        public int CountAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Count(*) AS Count FROM Groups";
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
