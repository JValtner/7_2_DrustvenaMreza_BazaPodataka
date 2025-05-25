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
        public ActionResult<Grupa> GetById(int grupaId)
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
            int rowsAfected = groupDbRepo.NewGroup(newGrupa);
            if (rowsAfected == 0 || string.IsNullOrWhiteSpace(newGrupa.Ime) || string.IsNullOrWhiteSpace(newGrupa.DatumOsnivanja.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            

            return Ok(newGrupa);
        }

        [HttpPut("{grupaId}")]
        public ActionResult<Grupa> Update(int grupaId, [FromBody] Grupa grupaAzurirana)
        {
            Grupa grupa = groupDbRepo.GetGroup(grupaId);
            grupa.Ime = grupaAzurirana.Ime;
            grupa.DatumOsnivanja = grupaAzurirana.DatumOsnivanja;
            int rowsAfected = groupDbRepo.UpdateGroup(grupaId,grupa);
            if ( string.IsNullOrWhiteSpace(grupaAzurirana.Ime) || string.IsNullOrWhiteSpace(grupaAzurirana.DatumOsnivanja.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            if (rowsAfected == 0 || grupa == null)
            {
                return NotFound();
            }
            return Ok(grupaAzurirana);
        }

        [HttpDelete("{grupaId}")]
        public ActionResult Delete(int grupaId)
        {
            int rowsAfected = groupDbRepo.DeleteGroup(grupaId); 
            if (rowsAfected == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
