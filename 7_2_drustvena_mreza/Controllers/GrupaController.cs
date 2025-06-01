using System.Runtime.CompilerServices;
using _6_1_drustvena_mreza.DOMEN;
using _6_1_drustvena_mreza.REPO;
using _7_2_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using static System.Reflection.Metadata.BlobBuilder;

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
        // Get api/groups/?page={page}&pageSize={pageSize}
        [HttpGet]
        public ActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize=10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and PageSize must be greater than zero.");
            }
            try
            {
                List<Grupa> grupe = groupDbRepo.GetPaged(page, pageSize);
                int totalCount = groupDbRepo.CountAll();
                if (grupe == null)
                {
                    return NotFound("Ne postoji ni jedna grupa");
                } 
                Object result = new
                {
                    Data = grupe,
                    TotalCount = totalCount
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem("Desila se greska tokom dobavljanja podataka");
            }
           
        }
        [HttpGet("{grupaId}")]
        public ActionResult<Grupa> GetById(int grupaId)
        {
            try
            {
                Grupa grupa = groupDbRepo.GetGroup(grupaId);
                if (grupa ==null)
                {
                    return NotFound("Takva grupa ne postoji");
                }
                return Ok(grupa);
            }
            catch (Exception ex)
            {
                return Problem("Desila se greska tokom dobavljanja podataka");
            }

        }
        
        // POST api/groups
        [HttpPost]
        public ActionResult<Grupa> Create([FromBody] Grupa newGrupa)
        {
            try
            {
                int rowsAfected = groupDbRepo.NewGroup(newGrupa);
                if (rowsAfected == 0 || rowsAfected == null || string.IsNullOrWhiteSpace(newGrupa.Ime) || string.IsNullOrWhiteSpace(newGrupa.DatumOsnivanja.ToString("yyyy-MM-dd")))
                {
                    return BadRequest("Neispravni podaci");
                }
                return Ok(newGrupa);
            }
            catch (Exception ex)
            {
                return Problem("Desila se greska tokom snimanja podataka");
            }
        }

        [HttpPut("{grupaId}")]
        public ActionResult<Grupa> Update(int grupaId, [FromBody] Grupa grupaAzurirana)
        {
            try
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
            catch (Exception ex)
            {
                return Problem("Desila se greska tokom azuriranja podataka");
            }

        }

        [HttpDelete("{grupaId}")]
        public ActionResult Delete(int grupaId)
        {
            try
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
            catch (Exception ex)
            {
                return Problem("Desila se greska tokom brisanja podataka");
            }

        }
    }
}
