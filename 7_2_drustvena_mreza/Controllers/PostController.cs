﻿using _7_2_drustvena_mreza.REPO;
using Microsoft.AspNetCore.Mvc;

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
