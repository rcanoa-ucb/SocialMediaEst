using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.DTOs;
using SocialMedia.Infrastructure.Validators;
using System.Net;

namespace SocialMedia.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public PostController(IPostService postService,
            IMapper mapper,
            IValidationService validationService)
        {
            _postService = postService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region Sin DTOs
        //[HttpGet]
        //public async Task<IActionResult> GetPost()
        //{
        //    var posts = await _postService.GetAllPostAsync();
        //    return Ok(posts);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetPostId(int id)
        //{
        //    var post = await _postService.GetPostAsync(id);
        //    return Ok(post);
        //}

        //[HttpPost]
        //public async Task<IActionResult> InsertPost(Post post)
        //{
        //    await _postService.InsertPostAsync(post);
        //    return Ok(post);
        //}
        #endregion

        #region Con DTO
        //[HttpGet("dto")]
        //public async Task<IActionResult> GetPostsDto()
        //{
        //    var posts = await _postService.GetAllPostAsync();
        //    var postsDto = posts.Select(p => new PostDto
        //    {
        //        Id = p.Id,
        //        UserId = p.UserId,
        //        Date = p.Date.ToString("dd-MM-yyyy"),
        //        Description = p.Description,
        //        Imagen = p.Imagen
        //    });

        //    return Ok(postsDto);
        //}

        //[HttpGet("dto/{id}")]
        //public async Task<IActionResult> GetPostIdDto(int id)
        //{
        //    var post = await _postService.GetPostAsync(id);
        //    var postDto = new PostDto
        //    {
        //        Id = post.Id,
        //        UserId = post.UserId,
        //        Date = post.Date.ToString("dd-MM-yyyy"),
        //        Description = post.Description,
        //        Imagen = post.Imagen
        //    };

        //    return Ok(postDto);
        //}

        //[HttpPost("dto")]
        //public async Task<IActionResult> InsertPostDto(PostDto postDto)
        //{
        //    var post = new Post
        //    {
        //        Id = postDto.Id,
        //        UserId = postDto.UserId,
        //        Date = Convert.ToDateTime(postDto.Date),
        //        Description = postDto.Description,
        //        Imagen = postDto.Imagen
        //    };

        //    await _postService.InsertPostAsync(post);
        //    return Ok(post);
        //}

        //[HttpPut("dto/{id}")]
        //public async Task<IActionResult> UpdatePostDto(int id, 
        //    [FromBody]PostDto postDto)
        //{
        //    if (id != postDto.Id)
        //        return BadRequest("El Id del Post no coincide");

        //    var post = await _postService.GetPostAsync(id);
        //    if (post == null)
        //        return NotFound("Post no encontrado");

        //    post.Id = postDto.Id;
        //    post.UserId = postDto.UserId;
        //    post.Date = Convert.ToDateTime(postDto.Date);
        //    post.Description = postDto.Description;
        //    post.Imagen = postDto.Imagen;

        //    await _postService.UpdatePostAsync(post);
        //    return Ok(post);
        //}

        //[HttpDelete("dto/{id}")]
        //public async Task<IActionResult> UpdatePostDto(int id)
        //{
        //    var post = await _postService.GetPostAsync(id);
        //    if (post == null)
        //        return NotFound("Post no encontrado");

        //    await _postService.DeletePostAsync(post);
        //    return NoContent();
        //}
        #endregion

        #region Dto Mapper
        /// <summary>
        /// Recupera una lista paginada de publicaciones como objetos de transferencia de datos segun filtro
        /// </summary>
        /// <remarks>
        /// Este metodo se utiliza para convertir las publicaciones recuperadas en DTOs que luego se 
        /// devuelven en registros paginados
        /// </remarks>
        /// <param name="postQueryFilter">Los filtros de aplican al recuperar las publicaciones como la paginacion y busqueda, 
        /// <param name="idAux">Identificador de la tabla</param>>
        /// si no se envian los parametros se retornan todos los registros</param>
        /// <returns>Coleccion o lista de post</returns>
        /// <responsecode="200">Retorna todos lo registros</responsecode>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetPostsDtoMapper(
            [FromQuery]PostQueryFilter postQueryFilter, int idAux)
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync(postQueryFilter);

                var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = posts.Pagination.TotalCount,
                    PageSize = posts.Pagination.PageSize,
                    CurrentPage = posts.Pagination.CurrentPage,
                    TotalPages = posts.Pagination.TotalPages,
                    HasNextPage = posts.Pagination.HasNextPage,
                    HasPreviousPage = posts.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<PostDto>>(postsDto)
                {
                    Pagination = pagination,
                    Messages = posts.Messages
                };

                return StatusCode((int)posts.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Error", Description = err.Message } },
                };
                return StatusCode(500, responsePost);
            }
        }

        [HttpGet("dto/dapper")]
        public async Task<IActionResult> GetPostsDtoMapper()
        {
            var posts = await _postService.GetAllPostDapperAsync();
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);

            var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);

            return Ok(response);
        }

        [HttpGet("dapper/1")]
        public async Task<IActionResult> GetPostCommentUserAsync()
        {
            var posts = await _postService.GetPostCommentUserAsync();
           

            var response = new ApiResponse<IEnumerable<PostComentariosUsersResponse>>(posts);

            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetPostsDtoMapperId(int id)
        {
            #region Validaciones
            var validationRequest = new GetByIdRequest { Id = id };
            var validationResult = await _validationService.ValidateAsync(validationRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Error de validación del ID",
                    Errors = validationResult.Errors
                });
            }
            #endregion

            var post = await _postService.GetPostAsync(id);
            var postDto = _mapper.Map<PostDto>(post);

            var response = new ApiResponse<PostDto>(postDto);

            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertPostDtoMapper([FromBody]PostDto postDto)
        {
            try
            {
                #region Validaciones
                // La validación automática se hace mediante el filtro
                // Esta validación manual es opcional
                var validationResult = await _validationService.ValidateAsync(postDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }
                #endregion

                var post = _mapper.Map<Post>(postDto);
                await _postService.InsertPostAsync(post);

                var response = new ApiResponse<Post>(post);

                return Ok(response);
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdatePostDtoMapper(int id,
            [FromBody] PostDto postDto)
        {
            if (id != postDto.Id)
                return BadRequest("El Id del Post no coincide");

            var post = await _postService.GetPostAsync(id);
            if (post == null)
                return NotFound("Post no encontrado");
      
            _mapper.Map(postDto, post);
            await _postService.UpdatePostAsync(post);

            var response = new ApiResponse<Post>(post);

            return Ok(response);
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeletePostDtoMapper(int id)
        {
            //var post = await _postService.GetPostAsync(id);
            //if (post == null)
            //    return NotFound("Post no encontrado");

            await _postService.DeletePostAsync(id);
            return NoContent();
        }
        #endregion
    }
}