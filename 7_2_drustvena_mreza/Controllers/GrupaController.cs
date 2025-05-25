using _6_1_drustvena_mreza.DOMEN;
using _6_1_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

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
            List<Grupa> grupe = GrupaRepo.grupaRepo.Values.ToList();
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
    }
}
