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
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly PostDbRepository _postRepository;

        // Konstruktor koji poziva ASP.NET i automatski dostavlja instancu za IConfiguration parametar
        public PostController(IConfiguration configuration)
        {
            _postRepository = new PostDbRepository(configuration);
        }
        // Get api/groups/?page={page}&pageSize={pageSize}
        [HttpGet]
        public ActionResult<List<Post>> GetAllPosts()
        {
            try
            {
                var posts = _postRepository.GetAll();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
