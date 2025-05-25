using System;
using _6_1_drustvena_mreza.DOMEN;
using _6_1_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Mvc;

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
            List<Korisnik> korisnici = KorisnikRepo.korisnikRepo.Values.ToList();
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
    }
}

