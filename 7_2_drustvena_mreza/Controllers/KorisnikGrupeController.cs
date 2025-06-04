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

        public KorisnikGrupeController(IConfiguration configuration)
        {
            userDbRepo = new UserDbRepo(configuration);
            groupDbRepo = new GroupDbRepo(configuration);
        }


        [HttpPost("{groupId}")]
        public ActionResult<Korisnik> AddUserToGroup(int korisnikId, int groupId)
        {
            
            return Ok();
        }

        [HttpDelete("{groupId}")]
        public ActionResult RemoveUserFromGroup(int korisnikId, int groupId)
        {

            
            return Ok($"User {korisnikId} was removed from group {groupId}");         
        }
    }
}


