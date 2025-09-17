using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.DTOs;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public PostController(IPostRepository postRepository,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
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

        #region Con DTO
        [HttpGet("dto")]
        public async Task<IActionResult> GetPostsDto()
        {
            var posts = await _postRepository.GetAllPostAsync();
            var postsDto = posts.Select(p => new PostDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Date = p.Date,
                Description = p.Description,
                Imagen = p.Imagen
            });

            return Ok(postsDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetPostIdDto(int id)
        {
            var post = await _postRepository.GetPostAsync(id);
            var postDto = new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Date = post.Date,
                Description = post.Description,
                Imagen = post.Imagen
            };

            return Ok(postDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertPostDto(PostDto postDto)
        {
            var post = new Post
            {
                Id = postDto.Id,
                UserId = postDto.UserId,
                Date = postDto.Date,
                Description = postDto.Description,
                Imagen = postDto.Imagen
            };

            await _postRepository.InsertPostAsync(post);
            return Ok(post);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdatePostDto(int id, 
            [FromBody]PostDto postDto)
        {
            if (id != postDto.Id)
                return BadRequest("El Id del Post no coincide");

            var post = await _postRepository.GetPostAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");

            post.Id = postDto.Id;
            post.UserId = postDto.UserId;
            post.Date = postDto.Date;
            post.Description = postDto.Description;
            post.Imagen = postDto.Imagen;

            await _postRepository.UpdatePostAsync(post);
            return Ok(post);
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> UpdatePostDto(int id)
        {
            var post = await _postRepository.GetPostAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");

            await _postRepository.DeletePostAsync(post);
            return NoContent();
        }
        #endregion

        #region Dto Mapper
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetPostsDtoMapper()
        {
            var posts = await _postRepository.GetAllPostAsync();
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);

            return Ok(postsDto);
        }

        #endregion
    }
}
