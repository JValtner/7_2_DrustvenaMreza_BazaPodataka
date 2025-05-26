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

        private readonly GroupDbRepo groupDbRepo;

        // Konstruktor koji poziva ASP.NET i automatski dostavlja instancu za IConfiguration parametar
        public GrupaController(IConfiguration configuration)
        {
            groupDbRepo = new GroupDbRepo(configuration);
        }
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
            if (rowsAfected == 0 || rowsAfected == null || string.IsNullOrWhiteSpace(newGrupa.Ime) || string.IsNullOrWhiteSpace(newGrupa.DatumOsnivanja.ToString("yyyy-MM-dd")))
            {
                return BadRequest("Neispravni podaci");
            }
            

            return Ok(newGrupa);
        }

        [HttpPut("{grupaId}")]
        public ActionResult<Grupa> Update(int grupaId, [FromBody] Grupa grupaAzurirana)
        {
            Grupa grupa = groupDbRepo.GetGroup(grupaId);
            if ( string.IsNullOrWhiteSpace(grupaAzurirana.Ime) || string.IsNullOrWhiteSpace(grupaAzurirana.DatumOsnivanja.ToString("yyyy-MM-dd")))
            {
                return BadRequest();
            }
            if (grupa == null)
            {
                return NotFound("Takva grupa ne postoji");
            }
            grupa.Ime = grupaAzurirana.Ime;
            grupa.DatumOsnivanja = grupaAzurirana.DatumOsnivanja;
            int rowsAfected = groupDbRepo.UpdateGroup(grupaId,grupa);
            
            if (rowsAfected == 0 || rowsAfected ==null || grupa == null)
            {
                return NotFound();
            }

            
            return Ok(grupaAzurirana);
        }

        [HttpDelete("{grupaId}")]
        public ActionResult Delete(int grupaId)
        {
            Grupa grupa = groupDbRepo.GetGroup(grupaId);
            if (grupa == null)
            {
                return NotFound("Takva grupa ne postoji");
            }
            int rowsAfected = groupDbRepo.DeleteGroup(grupaId); 
            if (rowsAfected == 0)
            {
                return NotFound("Grupa nije obrisana, doslo je do greske");
            }
            return NoContent();
        }
    }
}
