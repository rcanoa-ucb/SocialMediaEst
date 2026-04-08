using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Services.Interfaces;
using System.Net;

namespace SocialMedia.Services.Services
{
    public class PostService : IPostService
    {
        //public readonly IPostRepository _postRepository;
        //public readonly IUserRepository _userRepository;

        //public readonly IBaseRepository<Post> _postRepository;
        //public readonly IBaseRepository<User> _userRepository;

        public readonly IUnitOfWork _unitOfWork;

        //public PostService(IBaseRepository<Post> postRepository,
        //    IBaseRepository<User> userRepository)
        //{
        //    _postRepository = postRepository;
        //    _userRepository = userRepository;
        //}

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            //return await _postRepository.GetAll();
            //GetAllPostsAsync();

            return await _unitOfWork.PostRepository.GetAll();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
            // GetPostByIdAsync(id);
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
                throw new BussinesException("El usuario no existe",
                    HttpStatusCode.BadRequest);

            if (ContainsFobbidenWord(post.Description))
            {
                throw new BussinesException("El contenido no es permitido",
                    HttpStatusCode.BadRequest);
            }

            //Si el usuario tiene menos de 10 publicaciones,
            //solo puede publicar 1 sola vez por semana
            var userPost = await _unitOfWork.PostRepository
                .GetAllPostsByUserAsync(post.UserId);
            if (userPost.Count() < 10)
            {
                var lastPost = userPost
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefault();
                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BussinesException("No puedes publicar este post",
                        HttpStatusCode.BadRequest);
                }
            }

            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveChangesAsync();
        }

        public void UpdatePost(Post post)
        {
            //await _postRepository.Update(post
            _unitOfWork.PostRepository.Update(post);
            _unitOfWork.SaveChanges();
        }

        public async Task DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        //Lista de palabras o expresiones no permitidas
        public readonly string[] ForbbidenWords =
        {
            "violencia",
            "odio",
            "groseria",
            "discriminacion",
            "pornografía"
        };

        public bool ContainsFobbidenWord(string text)
        { 
            if (string.IsNullOrWhiteSpace(text))
                return false;

            foreach (var word in ForbbidenWords)
            { 
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
