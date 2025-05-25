using System.Runtime.CompilerServices;
using _6_1_drustvena_mreza.DOMEN;
using _6_1_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace _6_1_drustvena_mreza.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GrupaController : ControllerBase
    {
        private GrupaRepo grupaRepo = new GrupaRepo();
        private KorisnikRepo korisnikRepo = new KorisnikRepo();

        // Get api/groups
        [HttpGet]
        public ActionResult<List<Grupa>> GetAll()
        {
            List<Grupa> grupe = GetAllFromDatabase();
            if (grupe == null) 
            { 
                return NotFound("Ne postoji ni jedna grupa");
            }
            return Ok(grupe);
        }
        [HttpGet("{grupaId}")]
        public ActionResult<Korisnik> NadjiKorisnikaGrupeId(int grupaId)
        {
            if (!GrupaRepo.grupaRepo.ContainsKey(grupaId))
            {
                return NotFound("Takva grupa ne postoji");
            }
            List<Korisnik> listakorisnikaGrupe = korisnikRepo.NadjiKorisnike(grupaId);

            return Ok(listakorisnikaGrupe);
        }


        // POST api/groups
        [HttpPost]
        public ActionResult<Grupa> Create([FromBody] Grupa newGrupa)
        {
            if (string.IsNullOrWhiteSpace(newGrupa.Ime) || string.IsNullOrWhiteSpace(newGrupa.DatumOsnivanja.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            newGrupa.Id = SracunajNoviId(GrupaRepo.grupaRepo.Keys.ToList());
            GrupaRepo.grupaRepo[newGrupa.Id] = newGrupa;
            grupaRepo.Sacuvaj();

            return Ok(newGrupa);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!GrupaRepo.grupaRepo.ContainsKey(id))
            {
                return NotFound();
            }

            GrupaRepo.grupaRepo.Remove(id);
            grupaRepo.Sacuvaj();

            return NoContent();
        }

        private int SracunajNoviId(List<int> identifikatori)
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
        private List<Grupa> GetAllFromDatabase()
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

                    DateTime datumOsnivanja = DateTime.ParseExact(reader["CreationDate"].ToString(),"yyyy-MM-dd",System.Globalization.CultureInfo.InvariantCulture
);
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
    }
}
