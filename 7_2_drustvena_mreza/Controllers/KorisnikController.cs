using System;
using System.Text.RegularExpressions;
using _6_1_drustvena_mreza.DOMEN;
using _6_1_drustvena_mreza.REPO;
using _7_2_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace _6_1_Drustvena_Mreza.Controllers
{
    [Route("api/korisnik")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        //private KorisnikRepo korisnikRepo = new KorisnikRepo();
        private UserDbRepository userDbRepository = new UserDbRepository();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        { 
            try
            {
                List<Korisnik> korisnici = userDbRepository.GetAll();

                if (korisnici == null)
                {
                    return NotFound("No users found");
                }
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] An exception occurred in GetAll: {ex.Message}");
                return Problem("An unexpected error occurred while retrieving the list of users.");
            }
            
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            try
            {
                Korisnik korisnik = userDbRepository.GetById(id);

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
            try
            {
                if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Prezime) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd")))
                {
                    return BadRequest("Invalid input. All fields must be filled out.");
                }

                int lastID = userDbRepository.Create(noviKorisnik);

                if (lastID == 0)
                {
                    return Problem("Failed to create the user. Please try again.");
                }

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
            try
            {
                if (string.IsNullOrWhiteSpace(korisnikAzuriran.KorisnickoIme) || 
                    string.IsNullOrWhiteSpace(korisnikAzuriran.Ime) || 
                    string.IsNullOrWhiteSpace(korisnikAzuriran.Prezime) || 
                    string.IsNullOrWhiteSpace(korisnikAzuriran.DatumRodjenja.ToString("yyyy-MM-dd")))
                {
                    return BadRequest();
                }

                Korisnik korisnik = userDbRepository.GetById(id);

                if (korisnik == null)
                {
                    return NotFound("User with the specified ID was not found.");
                }

                korisnik.KorisnickoIme = korisnikAzuriran.KorisnickoIme;
                korisnik.Ime = korisnikAzuriran.Ime;
                korisnik.Prezime = korisnikAzuriran.Prezime;
                korisnik.DatumRodjenja = korisnikAzuriran.DatumRodjenja;

                int rowsAffected = userDbRepository.Update(id, korisnik);

                if (rowsAffected == 0)
                {
                    return Problem("Update failed. Please try again.");
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
                int rowsAfected = userDbRepository.Delete(id);

                if (rowsAfected == 0)
                {
                    return NotFound("User with the specified ID was not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] An exception occurred in Delete: {ex.Message}");
                return Problem("An unexpected error occurred while deleting the user.");
            }

           
        }      
               
    }
}

