using System;
using _6_1_drustvena_mreza.DOMEN;
using _6_1_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace _6_1_Drustvena_Mreza.Controllers
{
    [Route("api/korisnik")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private KorisnikRepo korisnikRepo = new KorisnikRepo();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            List<Korisnik> korisnici = GetAllFromDatabase();
            if (korisnici == null)
            {
                return NotFound("Ne postoji ni jedna grupa");
            }
            return Ok(korisnici);
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            if (!KorisnikRepo.korisnikRepo.ContainsKey(id))
            {
                return NotFound();
            }
            return Ok(KorisnikRepo.korisnikRepo[id]);
        }


        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime) || string.IsNullOrWhiteSpace(noviKorisnik.Prezime) || string.IsNullOrWhiteSpace(noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }

            noviKorisnik.Id = MaxId(KorisnikRepo.korisnikRepo.Keys.ToList());
            KorisnikRepo.korisnikRepo[noviKorisnik.Id] = noviKorisnik;
            korisnikRepo.Sacuvaj();

            return Ok(noviKorisnik);
        }


        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik korisnikAzuriran)
        {
            if (string.IsNullOrWhiteSpace(korisnikAzuriran.KorisnickoIme) || string.IsNullOrWhiteSpace(korisnikAzuriran.Ime) || string.IsNullOrWhiteSpace(korisnikAzuriran.Prezime) || string.IsNullOrWhiteSpace(korisnikAzuriran.DatumRodjenja.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            if (!KorisnikRepo.korisnikRepo.ContainsKey(id))
            {
                return NotFound();
            }

            Korisnik korisnik = KorisnikRepo.korisnikRepo[id];
            korisnik.KorisnickoIme = korisnikAzuriran.KorisnickoIme;
            korisnik.Ime = korisnikAzuriran.Ime;
            korisnik.Prezime = korisnikAzuriran.Prezime;
            korisnik.DatumRodjenja = korisnikAzuriran.DatumRodjenja;
            korisnikRepo.Sacuvaj();

            return Ok(korisnik);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!KorisnikRepo.korisnikRepo.ContainsKey(id))
            {
                return NotFound();
            }

            KorisnikRepo.korisnikRepo.Remove(id);
            korisnikRepo.Sacuvaj();

            return NoContent();
        }


        private int MaxId(List<int> identifikatori)
        {
            int maxId = 0;
            foreach (int id in identifikatori)
            {
                if (id > maxId)
                {
                    maxId = id;
                }
            }
            return maxId + 1;
        }

        private List<Korisnik> GetAllFromDatabase()
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
    }
}

