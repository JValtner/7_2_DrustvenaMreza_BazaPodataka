using System;
using System.Linq;
using _6_1_drustvena_mreza.DOMEN;
using _6_1_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Mvc;

namespace _6_1_Drustvena_Mreza.Controllers
{
    [Route("api/korisnik/{korisnikId}/grupa")]
    [ApiController]
    public class KorisnikGrupeController : ControllerBase
    {
        private KorisnikRepo korisnikRepo = new KorisnikRepo();
        private GrupaRepo grupaRepo = new GrupaRepo();


        
        [HttpPost("{groupId}")]
        public ActionResult<Korisnik> AddUserToGroup(int korisnikId, int groupId)
        {
            Korisnik korisnik = korisnikRepo.NadjiKorisnikaId(korisnikId);
            Grupa grupa = grupaRepo.NadjiGrupu(groupId);

            if (korisnik == null || !KorisnikRepo.korisnikRepo.ContainsKey(korisnikId))
            {
                return NotFound("Takav korisnik ne postoji");
            }
            if (!GrupaRepo.grupaRepo.ContainsKey(groupId))
            {
                return NotFound("Takva grupa ne postoji");
            }
            if (korisnik.GrupeKorisnika.Contains(grupa))
            {
                return NotFound("Korisnik vec pripada grupi");
            }

            korisnik.GrupeKorisnika.Add(grupa);
            korisnikRepo.Sacuvaj();

            return Ok(korisnik);
        }

        [HttpDelete("{groupId}")]
        public ActionResult RemoveUserFromGroup(int korisnikId, int groupId)
        {

            Korisnik korisnik = korisnikRepo.NadjiKorisnikaId(korisnikId);
            Grupa grupa = grupaRepo.NadjiGrupu(groupId);

            if (korisnik == null || !KorisnikRepo.korisnikRepo.ContainsKey(korisnikId))
            {
                return NotFound("Takav korisnik ne postoji");
            }
            if (!GrupaRepo.grupaRepo.ContainsKey(groupId))
            {
                return NotFound("Takva grupa ne postoji");
            }
            if (!korisnik.GrupeKorisnika.Contains(grupa))
            {
                return NotFound("Korisnik nije u grupi");
            }

            korisnik.GrupeKorisnika.Remove(grupa);
            korisnikRepo.Sacuvaj();

            return Ok($"User {korisnikId} was removed from group {groupId}");         
        }
    }
}


