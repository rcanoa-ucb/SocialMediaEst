using AutoMapper;
using Azure;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Services.Interfaces;
using SocialMedia.Services.Validators;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        //private readonly IPostRepository _postRepository;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly CrearPostDtoValidator _crearValidator;
        private readonly ActualizarPostDtoValidator _actualizarValidator;

        public PostController(
            //IPostRepository postRepository,
            IPostService postService,
            IMapper mapper,
            CrearPostDtoValidator crearValidator,
            ActualizarPostDtoValidator actualizarValidator)
        {
            //_postRepository = postRepository;
            _postService = postService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Sin DTOs
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPost(Post post)
        {
            await _postService.InsertPost(post);
            return Created($"api/post/{post.Id}", post);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(Post post)
        {
            _postService.UpdatePost(post);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePost(Post post)
        {
            await _postService.DeletePost(post.Id);
            return NoContent();
        }
        #endregion

        #region Con Dtos
        [HttpGet("dto")]
        public async Task<IActionResult> GetDtoPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
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
            var post = await _postService.GetPostByIdAsync(id);
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

            await _postService.InsertPost(post);
            return Created($"api/post/{post.Id}", post);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateDtoPost
            (int id, [FromBody]PostDto postDto)
        {
            if (id != postDto.Id)
                return BadRequest("El ID del post no coincide");

            var post = await _postService.GetPostByIdAsync(id);
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

            _postService.UpdatePost(post);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteDtoPost
            (int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");

            await _postService.DeletePost(post.Id);
            return NoContent();
        }
        #endregion

        #region Con Dto Mapper
        [HttpGet("dto/mapper")]
        //?userId=10 & Date = '10-01-2026' & param = 15
        public async Task<IActionResult> GetPostsDtoMapper(
            [FromQuery] PostQueryFilter? filters)
        {
            var posts = await _postService.GetAllPostsAsync(filters);
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);

            var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);

            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetPostByIdDtoMapper(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound("Post no encontrado.");

            var postDto = _mapper.Map<PostDto>(post);

            var response = new ApiResponse<PostDto>(postDto);

            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertPostDtoMapper(PostDto postDto)
        {
            //Validar el DTO
            var validationResult = await _crearValidator.ValidateAsync(postDto);
            if (!validationResult.IsValid)
            {
                //return BadRequest(new
                //{
                //    message = "Error de validación",
                //    errors = validationResult.Errors.Select(e => new 
                //    {
                //        field = e.PropertyName,
                //        error = e.ErrorMessage
                //        //errorcode = e.ErrorCode
                //    })
                //});

                // Lanzamos ValidationException de FluentValidation para que el Middleware la procese
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var post = _mapper.Map<Post>(postDto);
                await _postService.InsertPost(post);

                var response = new ApiResponse<PostDto>(postDto);
                return Ok(response);
            }
            catch (BussinesException)
            {
                //Re-lanzar para que le middleware lo capture y genere
                throw;
            }
            catch (Exception ex)
            {
                //Cualquier error inesperado
                throw new Exception("Error inesperado, intente mas tarde.", ex);
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdatePostDtoMapper(int id, [FromBody] PostDto postDto)
        {
            if (id != postDto.Id)
                return BadRequest("El ID del post no coincide.");

            //Validar el DTO
            var validationResult = await _actualizarValidator.ValidateAsync(postDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                        //errorcode = e.ErrorCode
                    })
                });
            }

            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound("Post no encontrado.");

            try
            {
                _mapper.Map(postDto, post);

                _postService.UpdatePost(post);
                var response = new ApiResponse<PostDto>(postDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al crear el post",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeletePostDtoMapper(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound("Post no encontrado.");

            await _postService.DeletePost(id);

            return NoContent(); // 204 sin contenido
        }
        #endregion
    }
}
