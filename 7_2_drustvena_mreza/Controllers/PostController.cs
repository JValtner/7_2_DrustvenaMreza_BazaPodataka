using _7_2_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _7_2_drustvena_mreza.DOMEN
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

        public class CreatePostDto
        {
            [JsonIgnore]
            public int Id { get; set; }
            [JsonIgnore]
            public int UserId { get; set; }
            [Required]
            public string Content { get; set; }
            [Required]
            public DateTime PostDate { get; set; }
        }

        [HttpPost("{userId}")]
        public ActionResult<CreatePostDto> AddPost( [FromBody] CreatePostDto noviPost, int userId )
        {
            try
            {
                var post = new Post
                (
                    noviPost.Id,
                    noviPost.UserId,
                    noviPost.Content,
                    noviPost.PostDate
                );
                
                Post kreiraniPost = _postRepository.NewPost(post, userId);
                return Ok(noviPost);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
