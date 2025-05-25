using System.Runtime.CompilerServices;
using _6_1_drustvena_mreza.DOMEN;
using _6_1_drustvena_mreza.REPO;
using _7_2_drustvena_mreza.REPO;
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
        private GroupDbRepo groupDbRepo = new GroupDbRepo();

        // Get api/groups
        [HttpGet]
        public ActionResult<List<Grupa>> GetAll()
        {
            List<Grupa> grupe = groupDbRepo.GetAll();
            if (grupe == null) 
            { 
                return NotFound("Ne postoji ni jedna grupa");
            }
            return Ok(grupe);
        }
        [HttpGet("{grupaId}")]
        public ActionResult<Grupa> GetGroup(int grupaId)
        {   
            Grupa grupa = groupDbRepo.GetGroup(grupaId);
            if (grupa ==null)
            {
                return NotFound("Takva grupa ne postoji");
            }
            return Ok(grupa);
        }
        
        // POST api/groups
        [HttpPost]
        public ActionResult<Grupa> Create([FromBody] Grupa newGrupa)
        {
            if (string.IsNullOrWhiteSpace(newGrupa.Ime) || string.IsNullOrWhiteSpace(newGrupa.DatumOsnivanja.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            groupDbRepo.NewGroup(newGrupa);

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
    }
}
