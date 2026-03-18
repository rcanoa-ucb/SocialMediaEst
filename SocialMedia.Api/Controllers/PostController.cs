using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

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
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPost(Post post)
        {
            await _postRepository.InsertPost(post);
            return Created($"api/post/{post.Id}", post);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(Post post)
        {
            await _postRepository.UpdatePost(post);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePost(Post post)
        {
            await _postRepository.DeletePost(post);
            return NoContent();
        }
        #endregion

        #region Con Dtos
        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoPosts()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            var postsDto = posts.Select(p => new PostDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Date = p.Date.ToString("dd-MM-yyyy"),
                Description = p.Description,
                Image = p.Imagen
            });


            return Ok(postsDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetDtoPostById(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            var postDto = new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Date = post.Date.ToString("dd-MM-yyyy HH:mm:ss"),
                Description = post.Description,
                Image = post.Imagen
            };
            return Ok(postDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertDtoPost(PostDto postDto)
        {
            var post = new Post
            {
                Id = postDto.Id,
                UserId = postDto.UserId,
                Date = Convert.ToDateTime(postDto.Date),
                Description = postDto.Description,
                Imagen = postDto.Image
            };

            await _postRepository.InsertPost(post);
            return Created($"api/post/{post.Id}", post);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoPost
            (int id, [FromBody]PostDto postDto)
        {
            if (id != postDto.Id)
                return BadRequest("El ID del post no coincide");

            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");

            //Mapear valor del DTO a la entidad
            post.UserId = postDto.UserId;
            post.Date = Convert.ToDateTime(postDto.Date);
            post.Description = postDto.Description;
            post.Imagen = postDto.Image;

            //var postDtoInsert = new Post
            //{
            //    Id = postDto.Id,
            //    UserId = postDto.UserId,
            //    Date = Convert.ToDateTime(postDto.Date),
            //    Description = postDto.Description,
            //    Imagen = postDto.Image
            //};

            await _postRepository.UpdatePost(post);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoPost
            (int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");

            await _postRepository.DeletePost(post);
            return NoContent();
        }
        #endregion

        #region Con Dto Mapper
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetDtoMapperPosts()
        {
            var posts = await _postRepository.GetAllPostsAsync();

            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);

            //var postsDto = posts.Select(p => new PostDto
            //{
            //    Id = p.Id,
            //    UserId = p.UserId,
            //    Date = p.Date.ToString("dd-MM-yyyy"),
            //    Description = p.Description,
            //    Image = p.Imagen
            //});


            return Ok(postsDto);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetDtoMapperPostById(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");

            var postDto = _mapper.Map<PostDto>(post);

            //var postDto = new PostDto
            //{
            //    Id = post.Id,
            //    UserId = post.UserId,
            //    Date = post.Date.ToString("dd-MM-yyyy HH:mm:ss"),
            //    Description = post.Description,
            //    Image = post.Imagen
            //};
            return Ok(postDto);
        }

        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertDtoMapperPost(PostDto postDto)
        {
            //var post = new Post
            //{
            //    Id = postDto.Id,
            //    UserId = postDto.UserId,
            //    Date = Convert.ToDateTime(postDto.Date),
            //    Description = postDto.Description,
            //    Imagen = postDto.Image
            //};

            var post = _mapper.Map<Post>(postDto);

            await _postRepository.InsertPost(post);
            return Created($"api/post/{post.Id}", post);
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateDtoMapperPost
            (int id, [FromBody] PostDto postDto)
        {
            if (id != postDto.Id)
                return BadRequest("El ID del post no coincide");

            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");

            //Mapear valor del DTO a la entidad
            //post.UserId = postDto.UserId;
            //post.Date = Convert.ToDateTime(postDto.Date);
            //post.Description = postDto.Description;
            //post.Imagen = postDto.Image;

            //var postDtoInsert = new Post
            //{
            //    Id = postDto.Id,
            //    UserId = postDto.UserId,
            //    Date = Convert.ToDateTime(postDto.Date),
            //    Description = postDto.Description,
            //    Imagen = postDto.Image
            //};

            _mapper.Map(postDto, post);

            await _postRepository.UpdatePost(post);
            return NoContent();
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteDtoMapperPost
            (int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");

            await _postRepository.DeletePost(post);
            return NoContent();
        }
        #endregion
    }
}
