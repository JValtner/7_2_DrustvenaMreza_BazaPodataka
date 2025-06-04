using System;
using System.Text.RegularExpressions;
using _7_2_drustvena_mreza.DOMEN;
using _7_2_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace _7_2_drustvena_mreza.Controllers
{
    [Route("api/korisnik")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        
        private readonly UserDbRepo userDbRepo;

        public KorisnikController(IConfiguration configuration)
        {
            userDbRepo = new UserDbRepo(configuration);
        }
        
        [HttpGet]
        public ActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and PageSize must be greater than zero.");
            }
            try
            {
                List<Korisnik> korisnici = userDbRepo.GetPaged(page,pageSize);
                int totalCount = userDbRepo.CountAll();
                if (korisnici== null)
                {
                    return NotFound("Ne postoji ni jedna grupa");
                }
                Object result = new
                {
                    Data = korisnici,
                    TotalCount = totalCount
                };
                return Ok(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] An exception occurred in GetPaged: {ex.Message}");
                return Problem("An unexpected error occurred while retrieving the list of users.");
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            try
            {
                Korisnik korisnik = userDbRepo.GetById(id);

                if (korisnik == null)
                {
                    return NotFound("User with the specified ID was not found.");
                }
                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] An exception occurred in GetById: {ex.Message}");
                return Problem("An unexpected error occurred while retrieving the user.");
            }
            
        }
        

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (    noviKorisnik == null ||
                    string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Prezime) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd")))
            {
                return BadRequest("Invalid input. All fields must be filled out.");
            }

            try
            {
                Korisnik kreiraniKorisnik = userDbRepo.Create(noviKorisnik);
                return Ok(noviKorisnik);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] An exception occurred in Create: {ex.Message}");
                return Problem("An unexpected error occurred while creating the user.");
            }
            
        }


        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik korisnikAzuriran)
        {
            if (    korisnikAzuriran == null ||
                    string.IsNullOrWhiteSpace(korisnikAzuriran.KorisnickoIme) ||
                    string.IsNullOrWhiteSpace(korisnikAzuriran.Ime) ||
                    string.IsNullOrWhiteSpace(korisnikAzuriran.Prezime) ||
                    string.IsNullOrWhiteSpace(korisnikAzuriran.DatumRodjenja.ToString("yyyy-MM-dd")))
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                korisnikAzuriran.Id = id;
                Korisnik korisnik = userDbRepo.Update(korisnikAzuriran);

                if (korisnik == null)
                {
                    return NotFound("User with the specified ID was not found.");
                }

                return Ok(korisnikAzuriran);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] An exception occurred in Update: {ex.Message}");
                return Problem("An unexpected error occurred while updating the user.");
            }
            
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool isDeleted = userDbRepo.Delete(id);

                if (isDeleted)
                {
                return NoContent();

                }

                return NotFound($"User with the specified ID {id} was not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] An exception occurred in Delete: {ex.Message}");
                return Problem("An unexpected error occurred while deleting the user.");
            }

           
        }      
               
    }
}

