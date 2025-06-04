using System;
using System.Text.RegularExpressions;
using _7_2_drustvena_mreza.DOMEN;
using _7_2_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace _7_2_drustvena_mreza.Controllers
{
    [Route("api/korisnik/{korisnikId}/grupa")]
    [ApiController]
    public class KorisnikGrupeController : ControllerBase
    {
        private readonly UserDbRepo userDbRepo;
        private readonly GroupDbRepo groupDbRepo;
        private readonly GroupMembershipRepo groupMembershipRepo;

        public KorisnikGrupeController(IConfiguration configuration)
        {
            userDbRepo = new UserDbRepo(configuration);
            groupDbRepo = new GroupDbRepo(configuration);
            groupMembershipRepo = new GroupMembershipRepo(configuration);
        }


        [HttpPost("{groupId}")]
        public ActionResult<Korisnik> AddUserToGroup(int korisnikId, int groupId)
        {
            try
            {
                int rowsAfected = groupMembershipRepo.NewMembership(korisnikId,groupId);
                if (rowsAfected == 0 || rowsAfected == null || korisnikId ==null || groupId ==null)
                {
                    return BadRequest("Neispravni podaci");
                }
                return Ok(rowsAfected);
            }
            catch (Exception ex)
            {
                return Problem("Desila se greska tokom snimanja podataka");
            }
        }

        [HttpDelete("{groupId}")]
        public ActionResult RemoveUserFromGroup(int korisnikId, int groupId)
        {
            try
            {
                int rowsAfected = groupMembershipRepo.DeleteMembership(korisnikId, groupId);
                if (rowsAfected == 0 || rowsAfected == null || korisnikId == null || groupId == null)
                {
                    return BadRequest("Neispravni podaci");
                }
                return Ok(rowsAfected);
            }
            catch (Exception ex)
            {
                return Problem("Desila se greska tokom snimanja podataka");
            }


            return Ok($"User {korisnikId} was removed from group {groupId}");         
        }
    }
}


