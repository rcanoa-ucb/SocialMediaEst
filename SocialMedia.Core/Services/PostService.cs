using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        //public readonly IPostRepository _postRepository;
        //public readonly IBaseRepository<Post> _postRepository;
        //public readonly IUserRepository _userRepository;
        //public readonly IBaseRepository<User> _userRepository;

        public readonly IUnitOfWork _unitOfWork;

        public PostService(
            //IPostRepository postRepository, 
            //IUserRepository userRepository) 
            //IBaseRepository<Post> postRepository,
            //IBaseRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            //_postRepository = postRepository;
            //_userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetAllPostsAsync(PostQueryFilter filters)
        {
            var posts = await _unitOfWork.PostRepository.GetAll();

            if (filters.userId != null)
            {
                posts = posts.Where(x => x.UserId == filters.userId);
            }
            if (filters.Date != null)
            {
                posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }
            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagedPosts = PagedList<object>.Create(posts, filters.PageNumber, filters.PageSize);
            if (pagedPosts.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de posts recuperados correctamente" } },
                    Pagination = pagedPosts,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedPosts,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }


        public async Task<IEnumerable<Post>> GetAllPostDapperAsync()
        {
            var posts = await _unitOfWork.PostRepository.GetAllPostDapperAsync(5);

            return posts;
        }

        public async Task<IEnumerable<PostComentariosUsersResponse>> GetPostCommentUserAsync()
        {
            var posts = await _unitOfWork.PostRepository.GetPostCommentUserAsync();

            return posts;
        }

        public async Task<Post> GetPostAsync(int id)
        {
            //return await _postRepository.GetById(id);
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public async Task InsertPostAsync(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new BussinesException("El usuario no existe");
            }

            if (ContainsForbiddenWord(post.Description))
            {
                throw new BussinesException("El contenido no es permitido");
            }
            //Si el usuario tiene menos de 10 publicaciones,
            //solo puede publicar 1 sola vez por semana
            var userPost = await _unitOfWork.PostRepository
                .GetAllPostByUserAsync(post.UserId);
            if (userPost.Count() < 7)
            {
                var lastPost = userPost.OrderByDescending(x => x.Date).FirstOrDefault();
                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BussinesException("No puedes publicar el post");
                }
            }

            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            await _unitOfWork.PostRepository.Update(post);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeletePostAsync(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        //Lista de palabras no permitidas
        public readonly string[] ForbiddensWords =
        {
            "violencia",
            "odio",
            "groseria",
            "discriminacion"
        };

        public bool ContainsForbiddenWord(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            foreach (var word in ForbiddensWords)
            { 
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

    }
}
