using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        #region Sin DTOs
        [HttpGet]
        public async Task<IActionResult> GetPost()
        {
            var posts = await _postRepository.GetAllPostAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostId(int id)
        {
            var post = await _postRepository.GetPostAsync(id);
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPost(Post post)
        {
            await _postRepository.InsertPostAsync(post);
            return Ok(post);
        } 
        #endregion



    }
}
