using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using _6_1_drustvena_mreza.DOMEN;
using Microsoft.Data.Sqlite;
namespace _7_2_drustvena_mreza.REPO
{
    public class GroupDbRepo
    {
        public List<Grupa> GetAll()
        {
            List<Grupa> listaGrupa = new List<Grupa>();
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);

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
                return null;
            }
            catch (FormatException ex)
            {
                return null;
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return null;
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                return null;
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

        }
        public Grupa GetGroup(int grupaId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
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
                return null;
            }
            catch (FormatException ex)
            {
                return null;
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return null;
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                return null;
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

        }
        public int NewGroup(Grupa grupa)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
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
        public int UpdateGroup(int grupaId,Grupa grupa)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
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

        public int DeleteGroup(int grupaId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=DATABASE/DrustveneMrezeDB.db");
                connection.Open();

                string query = "DELETE FROM Groups WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id",grupaId);
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
